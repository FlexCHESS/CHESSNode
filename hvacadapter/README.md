# Simulated HVAC /  BUILDING  Adapter
--------------------------------------------------------

This adapter emulates the operation of HVAC as virtual energy storage

## Build
--------

```
Docker build -t hvacadapter:latest .

```

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
