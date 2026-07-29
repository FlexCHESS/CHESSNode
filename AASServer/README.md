# AAS Server

Implements the [Asset Administration Shell: Part 2](http://industrialdigitaltwin.org/en/content-hub)
REST API (shells, submodels, registries and repositories - see `AssetAdministrationShellAPIApi.cs`,
`SubmodelAPIApi.cs`, `SubmodelRegistryAPIApi.cs`, `SubmodelRepositoryAPIApi.cs`), plus the
FlexCHESS CHESS network core operations in `CHESSNetworkController.cs`: registering CHESS assets
against the digital twin, exposing their capability/status, and running the cost-minimising
optimiser described below. It is the central per-node service other CHESS adapters and the
CoreAPI talk to (`http://aasserver.default.svc` inside the node's K3S cluster).

## Build and run

Native (from within this directory):

```
sh build.sh          # Linux/macOS
build.bat             # Windows
```

Docker:

```
cd src/IO.Swagger
docker build -t io.swagger .
docker run -p 5000:5000 io.swagger
```

## API operations

In addition to the standard AAS Part 2 REST API, `CHESSNetworkController.cs` adds:

| Method | Route | Purpose |
|--------|-------|---------|
| POST | `/register` | Register a CHESS asset (and its adapter) with the digital twin environment |
| DELETE | `/register` | Remove a registered CHESS / CHESS adapter |
| GET / POST | `/capability/{id}` | Get / update the capability of a registered CHESS in the digital twin |
| GET | `/status` | Get the flexibility status profile for all registered CHESS assets |
| GET / POST | `/status/{id}` | Get / request an update of the flexibility status profile for one CHESS asset |
| GET / POST | `/current` | Proxy to the EMS adapter's `/current` operation, aggregated per priority level |
| POST | `/run` | Invoke the optimiser synchronously with specified limits and objectives (per-priority-level KPI aggregation; see the `todo` noting the day-ahead scheduler below fills in the missing dispatch logic) |
| POST | `/run/dayahead` | Day-ahead cost-minimising scheduler - see below |

`PicloMarketController.cs` adds a [Piclo Flex marketplace](https://docs.picloflex.com) integration - see below:

| Method | Route | Purpose |
|--------|-------|---------|
| POST | `/piclo/assets` | Register/update local CHESS as Piclo Flex Assets |
| POST | `/piclo/planned-assets` | Register planned (not-yet-built) CHESS capacity as Piclo Planned Assets |
| POST | `/piclo/bids` | Submit Piclo bid ballots derived from a day-ahead schedule |

## Configuration

Environment variables read at startup (`Program.cs`):

| Variable | Purpose |
|----------|---------|
| `PFX_CERT_PATH` / `PFX_CERT_PASS` | Path and password for the node's TLS/UUDEX client certificate |
| `UUDEX_USER` / `UUDEX_PASS` | Credentials for the UUDEX message bus |
| `adtServiceUrl` | Azure Digital Twins instance URL |
| `adtClientId` / `adtClientSecret` / `adtTenantId` | Service principal credentials for the Digital Twins instance |
| `PICLO_CLIENT_ID` / `PICLO_API_KEY` | Piclo Flex machine user credentials (see below); the `/piclo/*` operations return 503 while unset |
| `PICLO_PROVIDER_ID` | Piclo Flex Provider ID, used to default the `provider` field on assets/planned assets when not supplied in the request |
| `PICLO_BASE_URL` | Overrides the Piclo API base URL (default `https://api.picloflex.com`) - e.g. to target Piclo's Experience/sandbox environment |

## Day-ahead cost-minimising optimiser

`POST /run/dayahead` (`CHESSNetworkController.runDayAheadPost`) computes a day-ahead
charge/discharge/curtailment schedule that keeps predicted grid import at or below a maximum
power demand limit while minimising predicted cost, then dispatches the resulting schedule to
each affected CHESS through the EMS adapter.

It queries the EMS adapter's `/current` operation once for the day's available flexible
capacity (each asset's available Wh and its per-cycle degradation cost), then greedily:

1. shaves every period where forecast demand exceeds the limit using the cheapest available
   discharge capacity first, and
2. replenishes the energy used during the cheapest-tariff periods that still have headroom
   under the limit.

The resulting per-asset schedule is POSTed to the EMS adapter's `/status/{id}` operation for
each CHESS that was allocated capacity (unless `Dispatch` is set to `false`, which returns the
computed plan without setting it). This is the same `/current` + `/status` pattern already used
by the real-time `/run` + EMS `polling()` loop, applied ahead of time over a forecast instead of
live telemetry.

Example request (24 hourly periods, a 5kW site import limit, and a day-ahead tariff):

```json
{
  "Limits": [{ "Name": "maxpower", "Unit": "W", "Value": 5000 }],
  "Demand": [3200, 3000, 2900, ... ],
  "Tariff": [0.1111, 0.1088, 0.1088, ... ],
  "PeriodHours": 1,
  "Recurrence": "daily",
  "Dispatch": true
}
```

The response reports the predicted total cost against a no-flexibility baseline cost, a
per-period breakdown (forecast demand, resulting grid import, tariff, cost, and any demand left
unserved by available capacity), and the concrete per-CHESS schedules that were dispatched.

## Piclo Flex marketplace integration

`PicloMarketController.cs` integrates with the [Piclo Flex marketplace API](https://docs.picloflex.com)
(`IO.Swagger.Piclo.PicloClient`), so CHESS flexibility can be registered and bid into Piclo's
flexibility competitions. Piclo authentication (`client_id`/`api_key` -> Bearer JWT, per
`POST /authtoken/v1/`) is handled transparently by `PicloClient`, which caches and renews the
token from `PICLO_CLIENT_ID`/`PICLO_API_KEY`.

* `POST /piclo/assets` creates or updates Piclo Flex Assets (`/assets/v1/`), one per request
  entry. An entry may reference a locally registered CHESS via `ChessId`, which is only used to
  confirm the CHESS is real and default `Status` to `"operational"` - all other Piclo fields
  (location, meter ids, MW capacities, technology classification, etc.) must be supplied
  explicitly, since Piclo's asset taxonomy and power ratings don't map from the CHESS digital
  twin's Wh-based flexibility model. Piclo has no upsert-by-ref operation, so the controller
  looks the `ref` up first (`GET /assets/v1/?ref=`) and creates or `PATCH`es accordingly.
* `POST /piclo/planned-assets` creates Piclo Planned Assets (`/planned-assets/v1/`) for CHESS
  capacity that hasn't been built yet - each entry is passed through as supplied, since Planned
  Assets have no live telemetry to enrich from.
* `POST /piclo/bids` submits bid ballots (`POST /bids/v1/competitions/{competitionId}/ballots/`)
  derived from a day-ahead schedule - typically the `DayAheadResult` returned by a prior
  `POST /run/dayahead` call (dispatched or not). The request supplies the Piclo Service Windows
  to bid into, each with the `[StartHour, EndHour)` slice of the day it covers:

  ```json
  {
    "DayAhead": { "...": "a DayAheadResult, e.g. from POST /run/dayahead" },
    "ServiceWindows": [
      { "CompetitionId": "z8mpC6x", "ServiceWindowId": "sW12gWx", "RateType": "utilisation", "StartHour": 16, "EndHour": 19 }
    ]
  }
  ```

  For each window, capacity is the average flexibility delivered (forecast demand minus
  resulting grid import) over that slice, in MW; unless `RateValue` overrides it, the offered
  rate (£/MW/h) is derived from the slice's average day-ahead tariff, times `MarginMultiplier`
  (default `1`, i.e. bid at cost). Windows with no capacity to offer are skipped rather than
  submitted as zero-capacity bids. One Ballot is submitted per distinct `CompetitionId`,
  covering all of that competition's (non-skipped) Service Window bids.
