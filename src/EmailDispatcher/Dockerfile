FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/EmailDispatcher/EmailDispatcher.csproj", "src/EmailDispatcher/"]
RUN dotnet restore "src/EmailDispatcher/EmailDispatcher.csproj"
COPY . .
WORKDIR "/src/src/EmailDispatcher"
RUN dotnet build "./EmailDispatcher.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EmailDispatcher.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmailDispatcher.dll"]
