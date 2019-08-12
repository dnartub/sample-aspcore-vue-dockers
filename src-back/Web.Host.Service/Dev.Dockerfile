FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY . .
WORKDIR /src/Web.Host.Service
RUN dotnet build Web.Host.Service.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Web.Host.Service.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Web.Host.Service.dll"]