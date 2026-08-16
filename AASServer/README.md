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
| POST | `/run/dayahead/flexibility` | Day-ahead scheduler sourced from an Energy Flexibility Data Model (EFDM) instance instead of live EMS adapter capacity - see below |
| POST | `/run/dayahead/flexibility/measures` | Express a computed day-ahead schedule as an EFDM `flexibleLoadMeasuresPackage` instance - see below |

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
  "Dispatch": true,
  "Options": [{
    "currentStatus":"available",
    "status":[{
      "status":"ForceCharge",
      "service":"all",
      "starttime":"02:00",
      "endtime":"06:00",
      "capacity":"1",
      "recurrence":"daily"
     },
     {
      "status":"ForceDischarge",
      "service":"all",
      "starttime":"07:15",
      "endtime":"11:15",
      "capacity":"1",
      "recurrence":"daily"
    },
    {
      "status":"ForceCharge",
      "service":"all",
      "starttime":"11:15",
      "endtime":"13:15",
      "capacity":"1",
      "recurrence":"daily"
    },
    {
      "status":"ForceDischarge",
      "service":"all",
      "starttime":"13:15",
      "endtime":"17:15",
      "capacity":"1",
      "recurrence":"daily"
    }]
  }]
}
```

The response reports the predicted total cost against a no-flexibility baseline cost, a
per-period breakdown (forecast demand, resulting grid import, tariff, cost, and any demand left
unserved by available capacity), and the concrete per-CHESS schedules that were dispatched.
## Energy Flexibility Data Model (EFDM) sourced day-ahead scheduler

`POST /run/dayahead/flexibility` (`CHESSNetworkController.runDayAheadFlexibilityPost`) computes
the same kind of day-ahead schedule as `/run/dayahead` above (same shave-then-replenish
allocation, same `DayAheadResult` response shape, same EMS adapter dispatch), but instead of
querying the EMS adapter's `/current` operation for live capacity, it derives each asset's
available flexibility from an [IDTA Energy Flexibility Data Model](https://industrialdigitaltwin.org/)
(EFDM) submodel instance supplied in the request body's `FlexibilitySubmodel` field - see
`EnergyFlexibilityDataModel.json` at the repo root for the template this instance should conform
to. `FlexibilitySubmodel` accepts either a bare `Submodel` object or a full AAS environment export
(`{ assetAdministrationShells, submodels, conceptDescriptions }`), in which case the first
submodel carrying a `flexibilitySpace_*` element is used.

For each `flexibleLoad` in the submodel's `flexibilitySpace_operationalPotential` (falling back to
`flexibilitySpace_applicationTailoredPotential`, then `flexibilitySpace_generalTechnicalPotential`
if the preferred one isn't present):

* its `powerStates` (a power range times a duration) are summed into discharge capacity
  (negative power) and/or charge capacity (positive power), in Wh;
* it's priced from `flexibleLoadCosts.variableCost`, following the same currency/kWh convention
  as `Tariff`;
* it's only eligible for the periods that fall inside its `validity.from`/`until` window, resolved
  against the request's `PlanStart` (default: today) - both are compared in UTC, so eligibility
  doesn't depend on the server's local timezone.

Storages and dependencies described in the EFDM instance are not yet incorporated into the
allocation.

Example request (one Flexible Load offering 2kW/4h of discharge capacity, priced at £0.05/kWh,
available all day):

```json
{
  "FlexibilitySubmodel": {
    "submodelElements": [{
      "idShort": "flexibilitySpace_operationalPotential",
      "value": [{
        "idShort": "flexibleLoads",
        "value": [{
          "idShort": "flexibleLoad",
          "value": [
            { "idShort": "flexibleLoadId", "value": "battery-1" },
            { "idShort": "validity", "value": [
              { "idShort": "from", "value": "2026-08-16T00:00:00Z" },
              { "idShort": "until", "value": "2026-08-17T00:00:00Z" }
            ]},
            { "idShort": "powerStates", "value": [{
              "idShort": "powerState",
              "value": [
                { "idShort": "power", "min": "-2000", "max": "0" },
                { "idShort": "duration", "min": "4", "max": "4" }
              ]
            }]},
            { "idShort": "flexibleLoadCosts", "value": [
              { "idShort": "variableCost", "value": "0.05" }
            ]}
          ]
        }]
      }]
    }]
  },
  "Limits": [{ "Name": "maxpower", "Unit": "W", "Value": 5000 }],
  "Demand": [3200, 3000, 2900, ... ],
  "Tariff": [0.1111, 0.1088, 0.1088, ... ],
  "PeriodHours": 1,
  "Recurrence": "daily",
  "Dispatch": true
}
```

`POST /run/dayahead/flexibility/measures` (`CHESSNetworkController.runDayAheadFlexibilityMeasuresPost`)
takes a `DayAheadResult` - typically returned by a prior `/run/dayahead` or
`/run/dayahead/flexibility` call - and re-expresses its `Schedules` as an EFDM
`flexibleLoadMeasuresPackage` instance: one `flexibleLoadMeasure` per scheduled asset, whose
`loadChangeProfile` traces the signed power (negative for discharge/demand-reduction, positive for
charge/demand-increase) at the start and end of each scheduled window. This lets a schedule
computed here be handed off to, or filed alongside, other EFDM-speaking systems in the data model
they expect.

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
