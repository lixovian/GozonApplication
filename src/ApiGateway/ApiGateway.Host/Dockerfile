FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ApiGateway/ApiGateway.Host/ApiGateway.Host.csproj", "src/ApiGateway/ApiGateway.Host/"]
RUN dotnet restore "src/ApiGateway/ApiGateway.Host/ApiGateway.Host.csproj"
COPY . .
WORKDIR "/src/src/ApiGateway/ApiGateway.Host"
RUN dotnet build "./ApiGateway.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ApiGateway.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiGateway.Host.dll"]
