# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:11.0 AS base
USER $APP_UID
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:11.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Lidemia.Scheduler/Lidemia.Scheduler.csproj","Lidemia.Scheduler/"]
COPY ["Lidemia.Common.BusinessLogic/Lidemia.Common.BusinessLogic.csproj","Lidemia.Common.BusinessLogic/"]
COPY ["Lidemia.Common/Lidemia.Common.csproj","Lidemia.Common/"]
COPY ["Lidemia.Core/Lidemia.Core.csproj","Lidemia.Core/"]
COPY ["Lidemia.Resources/Lidemia.Resources.csproj","Lidemia.Resources/"]
COPY ["Lidemia.DataAccess/Lidemia.DataAccess.csproj","Lidemia.DataAccess/"]
COPY ["Lidemia.EmailNotifications/Lidemia.EmailNotifications.csproj","Lidemia.EmailNotifications/"]
COPY ["Lidemia.Search/Lidemia.Search.csproj","Lidemia.Search/"]
RUN dotnet restore "./Lidemia.Scheduler/Lidemia.Scheduler.csproj"
COPY . .
WORKDIR "/src/Lidemia.Scheduler"
RUN dotnet build "./Lidemia.Scheduler.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Lidemia.Scheduler.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lidemia.Scheduler.dll"]