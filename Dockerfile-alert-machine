FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

RUN mkdir /alert_state_machine

# Copy csproj and restore
WORKDIR /
COPY alert_state_machine/alert_state_machine.csproj ./alert_state_machine
COPY utm-service/utm-service/utm-service.csproj ./utm-service/utm-service/

WORKDIR /utm-service/utm-service
RUN dotnet restore

WORKDIR /alert_state_machine
RUN dotnet restore

WORKDIR /
# Copy everything else and build
COPY . ./

WORKDIR /utm-service/utm-service
RUN dotnet build -c Release -o /alert_state_machine

WORKDIR /alert_state_machine
RUN dotnet build -c Release -o /app

WORKDIR /alert_state_machine
RUN dotnet publish -c Release -o /app

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "alert_state_machine.dll"]