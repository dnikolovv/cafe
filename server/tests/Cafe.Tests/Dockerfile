FROM endeveit/docker-jq AS config
WORKDIR /src
COPY . .
WORKDIR /src/tests/Cafe.Tests
RUN jq '.ConnectionStrings.DefaultConnection = "Server=relationaldb;Port=5432;Database=cafe-relational;User Id=postgres;Password=postgres;"' appsettings.json > tmp.$$.json && mv tmp.$$.json appsettings.json
RUN jq '.EventStore.ConnectionString = "Server=eventstore;Port=5432;Database=cafe-event-store;User Id=postgres;Password=postgres;"' appsettings.json > tmp.$$.json && mv tmp.$$.json appsettings.json
RUN cat appsettings.json

FROM microsoft/dotnet:2.2-sdk AS build

WORKDIR /src

COPY --from=config /src .

RUN dotnet restore Cafe.sln
RUN dotnet build Cafe.sln 

WORKDIR /src/tests/Cafe.Tests

RUN chmod +x run-tests-with-coverage-report.sh
CMD ./run-tests-with-coverage-report.sh \
 && cp coverage.* /coverage