FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app-webhost

COPY app-webhost/ /app-webhost/

ENTRYPOINT ["dotnet", "Web.Host.Service.dll"]