FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

# Copy csproj and restore
WORKDIR /
COPY DroneSimulator.API/DroneSimulator.API/DroneSimulator.API.csproj ./DroneSimulator.API/DroneSimulator.API/
COPY utm-service/utm-service/utm-service.csproj ./utm-service/utm-service/

WORKDIR /utm-service/utm-service
RUN dotnet restore

WORKDIR /DroneSimulator.API/DroneSimulator.API
RUN dotnet restore

WORKDIR /
# Copy everything else and build
COPY . ./

WORKDIR /utm-service/utm-service
RUN dotnet build -c Release -o /DroneSimulator.API/DroneSimulator.API

WORKDIR /DroneSimulator.API/DroneSimulator.API
RUN dotnet build -c Release -o /app

WORKDIR /DroneSimulator.API/DroneSimulator.API
RUN dotnet publish -c Release -o /app

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "DroneSimulator.API.dll"]

