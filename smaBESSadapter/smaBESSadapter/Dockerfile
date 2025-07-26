FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out



# buildtime container
FROM alpine:latest AS builder

ARG sbfspot_home=/usr/local/bin/sbfspot.3

# for SBFspot
RUN apk --no-cache add git make g++ boost-dev bluez-dev sqlite-dev mariadb-dev

WORKDIR /tmp
RUN git clone https://github.com/SBFspot/SBFspot.git

# Checkout latest "official" version
WORKDIR /tmp/SBFspot
RUN git checkout $(git describe --tags $(git rev-list --tags --max-count=1))

WORKDIR /tmp/SBFspot/SBFspot
RUN sed -i s-\#ifdef\ linux-\#ifdef\ linux\\n\#include\ \<sys\/select.h\>- Ethernet.h
RUN sed -i s-\#ifdef\ linux-\#ifdef\ linux\\n\#include\ \<sys\/select.h\>- bluetooth.h
RUN sed -i s/mariadbclient/mariadb/ makefile

RUN make nosql && make sqlite && make mysql && make mariadb

# install files
RUN make install_nosql && mv /usr/local/bin/sbfspot.3/SBFspot /usr/local/bin/sbfspot.3/SBFspot_nosql \
      && cp /tmp/SBFspot/SBFspot/sqlite/bin/SBFspot /usr/local/bin/sbfspot.3/SBFspot_sqlite \
      && cp /tmp/SBFspot/SBFspot/mysql/bin/SBFspot /usr/local/bin/sbfspot.3/SBFspot_mysql \
      && cp /tmp/SBFspot/SBFspot/mariadb/bin/SBFspot /usr/local/bin/sbfspot.3/SBFspot_mariadb \
      && cp /tmp/SBFspot/SBFspot/*.sql /usr/local/bin/sbfspot.3


# for SBFspotUploadDaemon
RUN apk --no-cache add curl-dev

WORKDIR /tmp/SBFspot/SBFspotUploadDaemon
RUN sed -i s/mariadbclient/mariadb/ makefile

RUN make sqlite && make mysql && make mariadb

#install files
RUN mkdir /etc/sbfspot /var/sbfspot
RUN make install_sqlite && mv $sbfspot_home/SBFspotUploadDaemon $sbfspot_home/SBFspotUploadDaemon_sqlite \
      && cp /tmp/SBFspot/SBFspotUploadDaemon/mysql/bin/SBFspotUploadDaemon $sbfspot_home/SBFspotUploadDaemon_mysql \
      && cp /tmp/SBFspot/SBFspotUploadDaemon/mariadb/bin/SBFspotUploadDaemon $sbfspot_home/SBFspotUploadDaemon_mariadb

# install database tools for copying to new image
RUN apk --no-cache add mariadb-client

##############################################################################################################



# build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
RUN apt -y update
RUN apt -y install build-essential zlib1g-dev libncurses5-dev libgdbm-dev libnss3-dev libssl-dev libreadline-dev libffi-dev libsqlite3-dev wget libbz2-dev


ARG confdir=/etc/sbfspot
ARG datadir=/var/sbfspot
ARG sbfspot_home=/usr/local/bin/sbfspot.3

ENV SBFSPOT_INTERVAL 300

# install Packages needed for SBFspot usage, tzdata for setting local time,
# mariadb-common, mariadb-connector-c and ncurses-libs for mysql command,
# sqlite for sqlite3 command, libcurl for SBFspotUploadDaemon,
# mosquitto-clients for mosquitto_pub, sed for setConfigValue,
# coreutils for timeout, bluez-deprecated for hcitool

RUN apt -y install libboost-all-dev bluez  tzdata     mariadb-common  libmariadb3 libmariadb-dev libcurl4-openssl-dev mosquitto-clients     sed coreutils

# copy SBFspot
COPY --from=builder $sbfspot_home $sbfspot_home
COPY ./docker/start.sh /
COPY ./deploy.sh /

# copy tools
COPY --from=builder /usr/bin/mysql /usr/bin/mysql

RUN mkdir $confdir $datadir 

RUN chmod g+w /var/log 
      
RUN chmod u+x /start.sh 
      

WORKDIR /app
COPY --from=build-env /app/out .


ENTRYPOINT ["dotnet", "smaBESSadapter.dll"]

