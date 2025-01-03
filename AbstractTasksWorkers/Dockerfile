FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
ENV DOTNET_URLS=http://+:5000
WORKDIR /app
EXPOSE 8090
EXPOSE 8091

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AbstractTasksWorkers/AbstractTasksWorkers.csproj", "AbstractTasksWorkers/"]
COPY ["AbstractTaskContracts/AbstractTaskContracts.csproj", "AbstractTaskContracts/"]
COPY ["AbstractTasksDal/AbstractTasksDal.csproj", "AbstractTasksDal/"]
COPY ["AbstractTasksDomain/AbstractTasksDomain.csproj", "AbstractTasksDomain/"]
RUN dotnet restore "AbstractTasksWorkers/AbstractTasksWorkers.csproj"
COPY . .
WORKDIR "/src/AbstractTasksWorkers"
RUN dotnet build "AbstractTasksWorkers.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AbstractTasksWorkers.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

COPY AbstractTasksWorkers/workers_appsettings.json /app/publish/

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AbstractTasksWorkers.dll"]
