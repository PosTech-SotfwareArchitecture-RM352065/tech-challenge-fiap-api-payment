#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0 AS base
WORKDIR /home/site/wwwroot
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Sanduba.Cloud.Function.MercadoPago/Sanduba.Cloud.Function.MercadoPago.csproj", "Sanduba.Cloud.Function.MercadoPago/"]
COPY ["Sanduba.Core.Application/Sanduba.Core.Application.csproj", "Sanduba.Core.Application/"]
COPY ["Sanduba.Core.Domain/Sanduba.Core.Domain.csproj", "Sanduba.Core.Domain/"]
COPY ["Sanduba.Infrastructure.Broker.ServiceBus/Sanduba.Infrastructure.Broker.ServiceBus.csproj", "Sanduba.Infrastructure.Broker.ServiceBus/"]
COPY ["Sanduba.Infrastructure.Persistence.MongoDb/Sanduba.Infrastructure.Persistence.MongoDb.csproj", "Sanduba.Infrastructure.Persistence.MongoDb/"]
COPY ["Sanduba.Infrastructure.MercadoPagoAPI/Sanduba.Infrastructure.MercadoPagoAPI.csproj", "Sanduba.Infrastructure.MercadoPagoAPI/"]
RUN dotnet restore "./Sanduba.Cloud.Function.MercadoPago/Sanduba.Cloud.Function.MercadoPago.csproj"
COPY . .
WORKDIR "/src/Sanduba.Cloud.Function.MercadoPago"
RUN dotnet build "./Sanduba.Cloud.Function.MercadoPago.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Sanduba.Cloud.Function.MercadoPago.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true