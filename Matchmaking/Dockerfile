FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /Matchmaking

# Development Stage: Restore dependencies and build the project
FROM base AS development
COPY *.csproj ./
RUN dotnet restore --use-current-runtime

COPY . ./
RUN dotnet build -c Debug --no-restore
ENTRYPOINT ["dotnet", "watch", "run", "--project", "Matchmaking.csproj"]

# Production Build Stage: Restore dependencies and publish the project
FROM base AS build
COPY *.csproj ./
RUN dotnet restore --use-current-runtime
COPY . ./
RUN dotnet publish -c Release -o /app/publish --use-current-runtime --self-contained false --no-restore

# Final Production Stage: Create the production-ready image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS production
WORKDIR /Matchmaking
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Matchmaking.dll"]