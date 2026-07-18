# Adapter for the BESS (SMA)
-----------------------------
The adapter implements the CHESS adapter contract for SMA solar/battery inverters. It is an
ASP.NET Core service (`Controllers/HomeController.cs`) that, on `/init`, runs `deploy.sh` to
configure and launch [SBFspot](https://github.com/SBFspot/SBFspot) - a third-party tool vendored
and built from source in this adapter's `Dockerfile` - which reads inverter data over
Bluetooth/Ethernet and publishes it via MQTT for ingestion into the digital twin. SBFspot's own
documentation and license are reproduced unmodified below.

## Build and run

Native .NET service (from within `smaBESSadapter/`):

```
dotnet build smaBESSadapter.sln
dotnet run --project smaBESSadapter
```

Docker (from within `smaBESSadapter/`) builds both the .NET service and SBFspot (from source,
via an Alpine build stage) together:

```
docker build -t smabessadapter:latest .
docker run -p 5000:80 smabessadapter:latest
```

## API operations

| Method | Route | Purpose |
|--------|-------|---------|
| POST | `/init` | Register a CHESS asset; runs `deploy.sh` to configure and start SBFspot/MQTT publishing for this asset, then starts its polling loop |
| GET / POST | `/status/{id}` | Get / set the schedule for a registered device |

## Configuration

Environment variables read at startup (`Program.cs`):

| Variable | Purpose |
|----------|---------|
| `AAS_URL` | Base URL of the AAS server used to push telemetry/status to the digital twin |

`deploy.sh` configures SBFspot itself, including the storage backend (`DB_STORAGE`), polling
interval (`SBFSPOT_INTERVAL`), and MQTT publishing (`MQTT_ENABLE`, `MQTT_PublisherArgs`, etc).

---

![SBFspot Logo](https://user-images.githubusercontent.com/1931158/30831762-006ec650-a249-11e7-86e3-13d01b36dd5d.jpg)

Translation by Google: [[NL](https://translate.google.com/translate?act=url&depth=1&hl=nl&ie=UTF8&prev=_t&rurl=translate.google.com&sl=en&sp=nmt4&tl=nl&u=https://github.com/SBFspot/SBFspot)] - [[FR](https://translate.google.com/translate?act=url&depth=1&hl=nl&ie=UTF8&prev=_t&rurl=translate.google.com&sl=en&sp=nmt4&tl=fr&u=https://github.com/SBFspot/SBFspot)] - [[DE](https://translate.google.com/translate?act=url&depth=1&hl=nl&ie=UTF8&prev=_t&rurl=translate.google.com&sl=en&sp=nmt4&tl=de&u=https://github.com/SBFspot/SBFspot)] - [[ES](https://translate.google.com/translate?act=url&depth=1&hl=nl&ie=UTF8&prev=_t&rurl=translate.google.com&sl=en&sp=nmt4&tl=es&u=https://github.com/SBFspot/SBFspot)] - [[IT](https://translate.google.com/translate?act=url&depth=1&hl=nl&ie=UTF8&prev=_t&rurl=translate.google.com&sl=en&sp=nmt4&tl=it&u=https://github.com/SBFspot/SBFspot)]
### **Introduction**
SBFspot, formerly known as [SMAspot](http://www.sma-spot.de/), is an open source project to get actual and archive data out of an SMA® inverter over Bluetooth or Ethernet (Speedwire®) It works on either Linux ([Raspberry Pi](http://www.raspberrypi.org)) and Windows.

### **What it does**
SBFspot connects via Bluetooth or Ethernet to your SMA® solar/battery inverter and reads Archive Day/Month Power generation, user/installer events and the actual (spot) data. The collected data is stored in a SQL database (SQLite/MySQL/MariaDB) or SMA® compatible .csv files.
A separate service/daemon uploads the collected data to [PVoutput.org](https://pvoutput.org)

### **Known bugs and limitations**
For a list of known bugs, consult the [issues](https://github.com/SBFspot/SBFspot/issues). If you find a bug, please create an [issue](https://github.com/SBFspot/SBFspot/issues).    
Don't create an issue for questions. Start a [discussion](https://github.com/SBFspot/SBFspot/discussions) instead.

[SMA Tripower X inverters](https://www.sma.de/en/products/solarinverters/sunny-tripower-x) are not supported by SBFspot. See [this discussion](https://github.com/SBFspot/SBFspot/discussions/634)

### **Documentation**
Refer to the [Wiki](https://github.com/SBFspot/SBFspot/wiki) for documentation and FAQ.

### **Special thanks to:**
* S. Pittaway: Author of ["NANODE SMA PV MONITOR"](https://github.com/stuartpittaway/nanodesmapvmonitor) on which this project is based.
* W. Simons : Early adopter, main tester and SMAdata2+® Protocol analyzer
* G. Schnuff : SMAdata2+® Protocol analyzer
* T. Frank: Speedwire® support
* Nakla: Author of original [docker container](https://github.com/nakla/sbfspot)
* All other users for their contribution - in any form - to the success of this project

### **Donations**
SBFspot is a free tool, developed during personal free time. If you like it, consider a donation.
If you click on the button below, you will be taken to the secure PayPal Web site. You don't need to have a paypal account in order to make a donation.

[![Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=3R5JSRCXBGSLQ)

### **License**
[Attribution - NonCommercial - ShareAlike 3.0 Unported (CC BY-NC-SA 3.0)](https://creativecommons.org/licenses/by-nc-sa/3.0/legalcode)

In short, you are free:
* to Share => to copy, distribute and transmit the work
* to Remix => to adapt the work
Under the following conditions:
* **Attribution:** You must attribute the work in the manner specified by the author or Licensor (but not in any way that suggests that they endorse you or your use of the work).
* **Noncommercial:** You may not use this work for commercial purposes.
* **Share Alike:** If you alter, transform, or build upon this work, you may distribute the resulting work only under the same or similar license to this one.

### **Disclaimer**
A user of SBFspot software acknowledges that he or she is receiving this software on an "as is" basis and the user is not relying on the accuracy or functionality of the software for any purpose. The user further acknowledges that any use of this software will be at his own risk and the copyright owner accepts no responsibility whatsoever arising from the use or application of the software.

SMA, Speedwire are registered trademarks of [SMA Solar Technology AG](http://www.sma.de/en/company/about-sma.html)
