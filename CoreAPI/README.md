# CHESS Core network APIs

The Core network APIs permit the registration, initialisation and access to adapters which virtualise and aggregate the CHESS assets.

The APIs are secured using Microgateways in the CHESS nodes. The CHESS nodes are deployed either on promises (edge) or in the cloud.

This CoreAPI implementation provides the REST Apis  defined in Swagger.yaml


## Build and Run

The core network API provider is packaged in Docker using the Dockerfile in src/IO.Swagger
 
The docker container is executed in the CHESS node which is a docker environment that supports lightweight K3S kubernetes (for adapter deployment). The deploy.sh and dcconfig.txt file contains the certificates necessary for adapter deployment. This should be moved to secrets or configmap files for poduction environments.

## Contact

tim@toshiba-bril.com
