# Adapter for the BESS (Huawei)
--------------------------------
The adapter implements the CHESS adapter contract for Huawei battery/solar inverters. It is two
cooperating processes packaged in one container:

- `huaweiBESSadapter/` - an ASP.NET Core service implementing the standard CHESS adapter API
  (`/init`, `/status/{id}`) and launching the Modbus/MQTT bridge below on `/init`.
- `huawei2mqtt.py` (with `modbus_energy_meter/`) - a Python bridge that polls a primary and
  secondary Huawei inverter over Modbus TCP (via the `huawei-solar` library) every ~60s and
  publishes the readings to an MQTT broker for ingestion into the digital twin.

## Build and run
----------------

Native .NET service (from within `huaweiBESSadapter/`):

```
dotnet build huaweiBESSadapter.sln
dotnet run --project huaweiBESSadapter
```

Native Python bridge (from within `huaweiBESSadapter/`), used standalone for testing:

```
python3.11 -m pip install -r requirements.txt
python3.11 huawei2mqtt.py
```

Docker (from within `huaweiBESSadapter/`) builds both together - the image installs Python 3.11
from source and the bridge's requirements alongside the .NET runtime:

```
docker build -t huaweibessadapter:latest .
docker run -p 5000:80 huaweibessadapter:latest
```

## API operations

| Method | Route | Purpose |
|--------|-------|---------|
| POST | `/init` | Register a CHESS asset; runs `deploy.sh` to configure and launch the Modbus/MQTT bridge for this asset, then starts its polling loop |
| GET / POST | `/status/{id}` | Get / set the schedule for a registered device |

## Configuration

Environment variables read at startup by the .NET service (`Program.cs`):

| Variable | Purpose |
|----------|---------|
| `AAS_URL` | Base URL of the AAS server used to push telemetry/status to the digital twin |

`deploy.sh` (invoked from `/init` with the CHESS id, location and MQTT topic) exports the
following for the Python bridge:

| Variable | Purpose |
|----------|---------|
| `HUAWEI_MODBUS_HOST` / `HUAWEI_MODBUS_PORT` | Address of the inverter's Modbus TCP interface |
| `HUAWEI_MODBUS_DEVICE_ID_SECONDARY` | Modbus slave ID of the secondary inverter (the primary ID is passed separately) |
| `HUAWEI_MODBUS_MQTT_TOPIC_PRIMARY` / `HUAWEI_MODBUS_MQTT_TOPIC_SECONDARY` | MQTT topics telemetry is published to |
| `HUAWEI_MODBUS_MQTT_USER` / `HUAWEI_MODBUS_MQTT_PASSWORD` / `HUAWEI_MODBUS_MQTT_BROKER` | MQTT broker connection details |
| `HUAWEI_MODBUS_DEBUG` | Set to `yes` for verbose bridge logging |

## Register CHESS

Register the adapter using the CoreAPI `/register` operation, e.g.:

```
{"adapter":{
   "Identifier":"huaweibessadapter",
   "Location":"CHESS Node 2",
   "Standard":"Modbus",
   "Version":"1.0",
   "Id":"huaweibessadapter",
   "Wireless":"test",
   "Container":"timfa/huaweibessadapter:latest",
   "Credentials":"default",
   "EnvConf":"<additional subject attributes>",
   "ExposedPort":80,
   "VolumeMount":""
  },
 "chess":[{
   "Identifier":"huaweibessadapter",
   "Location":"<location>",
   "Standard":"MQTT",
   "Version":"1.0",
   "Id":"<chess digital twin id>"
   }
]}
```
