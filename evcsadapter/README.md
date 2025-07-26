# Charging Station Monitoring and Control System Adapter
--------------------------------------------------------
Single endpoint to receive bulk updates from a Charging Station Management System (CSMS). 
The server is expected to update its internal database based on the differential changes in the data and respond with a list of load curves (one per charging station). A call to this endpoint is made as soon as new data is available or if not data is available on a set interval. 

## Build
--------

```
Docker build -t evcsadapter:latest .

```

## Control limits of CHESS
--------------------------

Using the /status/{id} API operation, with id being the CS + EVSE Id of the charge station.
For instance, id = ETDW_20247 and entry 1 corresponding to ETDW_20247-1 being the EVSE 1 of charging station ETDW_20247.
With entry 2 corresponding to ETDW_20247-2 being the EVSE 2 of the charging station ETDW_20247.

```
{
    "identifier":"evcsadapter",
    "location":"CHESS Node 2",
    "currentStatus":"available",
    "status":[{
      "status":"curtail",
      "service":"all",
      "starttime":"10:30",
      "endtime":"11:30",
      "capacity":"4000",
      "recurrence":"daily"
  },
  {
      "status":"curtail",
      "service":"all",
      "starttime":"10:40",
      "endtime":"11:40",
      "capacity":"4000",
      "recurrence":"daily"
 }]
}
```
