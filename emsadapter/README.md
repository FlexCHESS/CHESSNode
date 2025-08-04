# Smart Energy Management System Adapter
-----------------------------------------
Provides Energy Management System (EMS) functions and /ems API endpoint. 
The EMS adapter can estimate the cost and priority of the different assets in support of flexibility service targets, such as peak shaving and load shifting. 

## Build
--------

```
Docker build -t emsadapter:latest .

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
   "Identifier":"emsadapter",
   "Location":"<location>",
   "Standard":"REST",
   "Version":"1.0",
   "Id":"it-test-chess1-sim"
   }
]}
```
