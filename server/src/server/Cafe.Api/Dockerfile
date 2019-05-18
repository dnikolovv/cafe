FROM endeveit/docker-jq AS config

EXPOSE 80
EXPOSE 443

WORKDIR /server
COPY . .
WORKDIR /server/src/server/Cafe.Api

RUN jq '.ConnectionStrings.DefaultConnection = "Server=relationaldb;Port=5432;Database=cafe-relational;User Id=postgres;Password=postgres;"' appsettings.json > tmp.$$.json && mv tmp.$$.json appsettings.json
RUN jq '.EventStore.ConnectionString = "Server=eventstore;Port=5432;Database=cafe-event-store;User Id=postgres;Password=postgres;"' appsettings.json > tmp.$$.json && mv tmp.$$.json appsettings.json
RUN cat appsettings.json

FROM microsoft/dotnet:2.2-sdk AS build

WORKDIR /server

COPY --from=config /server .

WORKDIR /server/src/server/Cafe.Api
RUN dotnet publish Cafe.Api.csproj --configuration Release --output ./artifacts

WORKDIR /server/src/server/Cafe.Api/artifacts
RUN cat appsettings.json
ENTRYPOINT [ "dotnet", "Cafe.Api.dll" ]