# Charging Station Monitoring and Control System Adapter
--------------------------------------------------------
Single endpoint to receive bulk updates from a Charging Station Management System (CSMS). 
The server is expected to update its internal database based on the differential changes in the data and respond with a list of load curves (one per charging station). A call to this endpoint is made as soon as new data is available or if not data is available on a set interval. 

## Build
--------

```
Docker build -t evcsadapter:latest .

```

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
   "Identifier":"evcsadapter",
   "Location":"<location>",
   "Standard":"REST",
   "Version":"1.0",
   "Id":"it-test-chess1-sim"
   }
]}
```
