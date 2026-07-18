# Smart Energy Management System Adapter
-----------------------------------------
Provides Energy Management System (EMS) functions for the other CHESS assets registered on a
node: estimating the available flexible capacity, per-cycle degradation cost and priority of
each asset, and (via the `/status/{id}` polling loop) reactively dispatching them in support of
flexibility service targets such as peak shaving and load shifting.

## Build and run
----------------

Native (from within this directory):

```
sh build.sh          # Linux/macOS
build.bat             # Windows
```

Docker:

```
cd src/IO.Swagger
docker build -t emsadapter:latest .
docker run -p 5000:5000 emsadapter:latest
```

## API operations

| Method | Route | Purpose |
|--------|-------|---------|
| POST | `/init` | Register a CHESS asset with this adapter instance |
| POST | `/status/{id}` | Set a schedule for a registered CHESS asset; if `limit` (W) is also supplied, starts a real-time 60s polling loop that curtails/dispatches assets to keep total site power under that limit |
| GET | `/status/{id}` | Get the current schedule for a registered CHESS asset |
| POST | `/current` | For each requested time window in the body, compute the available flexible capacity (Wh), per-cycle degradation cost, and priority ranking of every registered CHESS asset |
| GET | `/current` | List the currently available flexibility provision, optionally filtered by location/recurrence/service |
| GET | `/history` | Return recorded power time series (total/BESS/PV/EVCS/HVAC/flex), optionally resetting the buffers |

## Day-ahead cost-minimising schedules

This adapter is the data source for [AASServer's `POST /run/dayahead`](../AASServer/README.md#day-ahead-cost-minimising-optimiser),
which queries this adapter's `/current` operation to price each registered asset's available
flexibility, computes a day-ahead charge/discharge schedule that keeps predicted grid import
under a maximum power limit while minimising predicted cost, and dispatches the result back to
each asset through this adapter's `/status/{id}` operation. Real-time enforcement of a power
limit (the `polling()` loop started from `/status/{id}?limit=...`) is a separate, complementary
mechanism - see the AASServer README for how the two fit together.

## Configuration

Environment variables read at startup (`Program.cs`), typically set via the CoreAPI `/register` `EnvConf` field or the deployment yaml:

| Variable | Purpose |
|----------|---------|
| `adtServiceUrl` | Azure Digital Twins instance URL |
| `adtClientId` / `adtClientSecret` / `adtTenantId` | Service principal credentials for the Digital Twins instance |
| `CONF` | Subject attributes passed to the digital twin on registration |

## Deploy to CHESS node
-----------------------
Using the /register Core API operation
```
{"adapter":{
   "Identifier":"emsadapter",
   "Location":"CHESS Node 1",
   "Standard":"REST",
   "Version":"1.0",
   "Id":"emsadapter",
   "Wireless":"test",
   "Container":"timfa/emsadapter:latest",
   "Credentials":"default",
   "EnvConf":"saFlexibilityProvideraaa-bbb-ccc-ddd",
   "ExposedPort":80,
   "VolumeMount":""
  },
 "chess":[{
   "Identifier":"emsadapter",
   "Location":"<location>",
   "Standard":"REST",
   "Version":"1.0",
   "Id":"it-test-chess1-sim"
   }
]}
```
