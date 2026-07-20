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

## Configuration

Environment variables read at startup (`Program.cs`):

| Variable | Purpose |
|----------|---------|
| `PFX_CERT_PATH` / `PFX_CERT_PASS` | Path and password for the node's TLS/UUDEX client certificate |
| `UUDEX_USER` / `UUDEX_PASS` | Credentials for the UUDEX message bus |
| `adtServiceUrl` | Azure Digital Twins instance URL |
| `adtClientId` / `adtClientSecret` / `adtTenantId` | Service principal credentials for the Digital Twins instance |

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
