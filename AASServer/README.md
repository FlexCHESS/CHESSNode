# IO.Swagger - ASP.NET Core 2.0 Server

All APIs of the Specification of the [Specification of the Asset Administration Shell: Part 2](http://industrialdigitaltwin.org/en/content-hub) in one collection.
## Run

Linux/OS X:

```
sh build.sh
```

Windows:

```
build.bat
```

## Run in Docker

```
cd src/IO.Swagger
docker build -t io.swagger .
docker run -p 5000:5000 io.swagger
```

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
