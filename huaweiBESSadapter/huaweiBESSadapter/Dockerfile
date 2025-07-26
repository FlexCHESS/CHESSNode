FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
RUN apt -y update
RUN apt -y install build-essential zlib1g-dev libncurses5-dev libgdbm-dev libnss3-dev libssl-dev libreadline-dev libffi-dev libsqlite3-dev wget libbz2-dev


WORKDIR /app
RUN wget https://www.python.org/ftp/python/3.11.0/Python-3.11.0.tgz
RUN tar -zxvf Python-3.11.0.tgz
WORKDIR /app/Python-3.11.0
RUN ./configure --enable-optimizations
RUN make -j$(nproc)
RUN make altinstall

WORKDIR /app
COPY --from=build-env /app/out .

COPY requirements.txt /app

RUN python3.11 -m pip install --upgrade pip && python3.11 -m pip install -r requirements.txt

#COPY .env.docker /app/.env
COPY modbus_energy_meter /app/modbus_energy_meter
COPY huawei2mqtt.py /app
ADD deploy.sh /app


ENTRYPOINT ["dotnet", "huaweiBESSadapter.dll"]

