FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /OpenChess

COPY *.csproj ./
RUN dotnet restore --use-current-runtime

COPY . ./
RUN dotnet publish -c Release -o out --use-current-runtime --self-contained false --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /OpenChess
COPY --from=build-env /OpenChess/out .
ENTRYPOINT ["dotnet", "OpenChess.dll"]
