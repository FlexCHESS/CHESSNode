# Build context is the repository root (see README: `docker build -f
# src/IO.Swagger/Dockerfile .`), so both this project and web/chess-dashboard
# are visible to COPY.

# --- CHESS Flexibility Dashboard (React/Vite) -------------------------------
FROM node:22-alpine AS web-build
WORKDIR /web

COPY web/chess-dashboard/package.json web/chess-dashboard/package-lock.json ./
RUN npm ci

COPY web/chess-dashboard/. ./
# Built under /dashboard/ since it's served from the API's wwwroot/dashboard,
# not the site root - keeps the generated asset URLs correct.
RUN npm run build -- --base=/dashboard/

# --- AAS Server (ASP.NET Core) -----------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# copy csproj and restore as distinct layers
COPY src/IO.Swagger/*.csproj ./
RUN dotnet restore

# copy everything else and build
COPY src/IO.Swagger/. ./
RUN dotnet publish -c Release -o out
ADD src/IO.Swagger/dcconfig.txt out
ADD src/IO.Swagger/deploy.sh out
ADD src/IO.Swagger/kubectl.gz out
ADD src/IO.Swagger/chess.yaml out
ADD src/IO.Swagger/uudex.pfx out
RUN gunzip out/kubectl.gz
RUN chmod a+x out/kubectl
RUN chmod a+x out/deploy.sh

# build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

COPY --from=build-env /app/out .
COPY --from=web-build /web/dist ./wwwroot/dashboard

ENTRYPOINT ["dotnet", "IO.Swagger.dll"]
