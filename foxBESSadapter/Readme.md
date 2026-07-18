# Adapter for the BESS  (FoxESS)
--------------------------------
The adapter implements the handlers for the interaction with BESS via the FoxESS Cloud service

## Build and run
----------------

Native (from within `foxBESSadapter/`):

```
dotnet build foxBESSadapter.sln
dotnet run --project foxBESSadapter
```

Docker (from within `foxBESSadapter/`):

```
docker build -t foxbessadapter:latest .
docker run -p 5000:80 foxbessadapter:latest
```

## API operations

| Method | Route | Purpose |
|--------|-------|---------|
| POST | `/init` | Register a CHESS asset; looks up the matching device on the FoxESS Cloud device list and starts its polling loop |
| GET / POST | `/status/{id}` | Get / set the schedule for a registered device |

## Configuration

Environment variables read at startup (`Program.cs`):

| Variable | Purpose |
|----------|---------|
| `CONF` | FoxESS Cloud API key, used to sign requests to `https://www.foxesscloud.com` |
| `AAS_URL` | Base URL of the AAS server used to push telemetry/status to the digital twin |
| `UUDEX_USER` / `UUDEX_PASS` | Credentials for the UUDEX message bus |

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
