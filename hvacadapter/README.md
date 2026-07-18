# Simulated HVAC /  BUILDING  Adapter
--------------------------------------------------------

This adapter emulates the operation of HVAC as virtual energy storage, using a building thermal
model together with an [Open-Meteo](https://open-meteo.com/) weather forecast (temperature and
irradiance) to predict PV generation and indoor temperature impact when deciding whether a
requested charge/discharge window can be met.

## Build and run
----------------

Native (from within `hvacadapter/`):

```
dotnet build hvacadapter.sln
dotnet run --project hvacadapter
```

Docker (from within `hvacadapter/`):

```
docker build -t hvacadapter:latest .
docker run -p 5000:80 hvacadapter:latest
```

## API operations

| Method | Route | Purpose |
|--------|-------|---------|
| POST | `/init` | Register a CHESS asset with this adapter instance |
| GET / POST | `/status/{id}` | Get / set the schedule for a registered device |

## Configuration

Environment variables read at startup (`Program.cs`):

| Variable | Purpose |
|----------|---------|
| `AAS_URL` | Base URL of the AAS server used to push telemetry/status to the digital twin |

No API key is required for the Open-Meteo forecast call.

## Control limits of CHESS
--------------------------
The status structure contains the requested scheduled energy for moving the HVAC energy usage..

```
{
    "identifier":"hvacadapter",
    "location":"CHESS Node 2",
    "currentStatus":"available",
    "status":[{
      "status":"ForceCharge",
      "service":"all",
      "starttime":"10:30",
      "endtime":"11:30",
      "capacity":"4000",
      "recurrence":"daily"
  },
  {
      "status":"ForceDischarge",
      "service":"all",
      "starttime":"12:40",
      "endtime":"13:40",
      "capacity":"4000",
      "recurrence":"daily"
 }]
}
```
