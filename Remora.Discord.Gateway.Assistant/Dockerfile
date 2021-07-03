FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS publish
WORKDIR /src
COPY /Remora.Discord.Gateway.Assistant .
RUN dotnet publish -c Release

FROM base
WORKDIR /app
COPY --from=publish /src/bin/Release/net5.0/publish .
ENTRYPOINT dotnet Remora.Discord.Gateway.Assistant.dll
