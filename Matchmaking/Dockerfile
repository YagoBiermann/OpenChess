FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /Matchmaking

COPY *.csproj ./
RUN dotnet restore --use-current-runtime

COPY . ./
RUN dotnet publish -c Release -o out --use-current-runtime --self-contained false --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /Matchmaking
COPY --from=build-env /Matchmaking/out .
ENTRYPOINT ["dotnet", "Matchmaking.dll"]
