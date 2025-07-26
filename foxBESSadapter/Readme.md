# Adapter for the BESS  (FoxESS)
--------------------------------
The adapter implements the handlers for the interaction with BESS via the FoxESS Cloud service

## Build
--------

```
Docker build -t foxbessadapter:latest .

```

## Update of the Digital twin data
----------------------------------

Using the /update Core API operation
```
Register the CHESS using the /register POST operation with:

{"adapter":{
   "Identifier":"foxbessadapter",
   "Location":"CHESS Node 2",
   "Standard":"MQTT",
   "Version":"1.0",
   "Id":"foxbessadapter",
   "Wireless":"test",
   "Container":"beaconacr.azurecr.io/foxbessadapter:latest",
   "Credentials":"beacon",
   "EnvConf":"<additional subject attributes>",
   "ExposedPort":80,
   "VolumeMount":""
  },
 "chess":[{
   "Identifier":"foxbessadapter",
   "Location":"<location>",
   "Standard":"REST",
   "Version":"1.0",
   "Id":"it-bess-blg1-chess1"
   }
]}
```
