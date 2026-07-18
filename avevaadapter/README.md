# Adapter for receiving data from HVAC/PV of the building
--------------------------------------------------------
The adapter receives /update POST messages containing the HVAC/PV data from the Aveva-pi

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
docker build -t avevaadapter:latest .
docker run -p 5000:5000 avevaadapter:latest
```

## API operations

| Method | Route | Purpose |
|--------|-------|---------|
| POST | `/init` | Register a CHESS asset with this adapter instance |
| GET / POST | `/status/{id}` | Get / set the schedule for a registered CHESS asset |
| POST | `/update` | Receive HVAC/PV telemetry pushed from the Aveva PI historian and forward it to the digital twin |

## Configuration

Environment variables read at startup (`Program.cs`), typically set via the CoreAPI `/register` `EnvConf` field or the deployment yaml:

| Variable | Purpose |
|----------|---------|
| `adtServiceUrl` | Azure Digital Twins instance URL |
| `adtClientId` / `adtClientSecret` / `adtTenantId` | Service principal credentials for the Digital Twins instance |
| `CONF` | Subject attributes passed to the digital twin on registration |

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
