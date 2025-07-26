# Adapter for receiving data from HVAC/PV of the building
--------------------------------------------------------
The adapter receives /update POST messages containing the HVAC/PV data from the Aveva-pi

## Build
--------

```
Docker build -t avevaadapter:latest .

```

## Update of the Digital twin data
----------------------------------

Using the /update Core API operation
```
Register the CHESS using the /register POST operation with:

{"adapter":{
   "Identifier":"avevaadapter",
   "Location":"CHESS Node 2",
   "Standard":"IREN",
   "Version":"1.0",
   "Id":"avevaadapter",
   "Wireless":"test",
   "Container":"timfa/avevaadapter:latest",
   "Credentials":"default",
   "EnvConf":"<additional subject attributes>",
   "ExposedPort":80,
   "VolumeMount":""
  },
 "chess":[{
   "Identifier":"evcsadapter",
   "Location":"<location>",
   "Standard":"REST",
   "Version":"1.0",
   "Id":"it-iren-chess1"
   }
]}
```
