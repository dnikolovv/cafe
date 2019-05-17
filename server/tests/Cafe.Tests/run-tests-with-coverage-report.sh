#!/bin/bash
dotnet test Cafe.Tests.csproj -p:Exclude="[xunit*]*%2c[Cafe.Persistance.Migrations]" -p:CollectCoverage=true -p:CoverletOutputFormat=opencover --logger:"console;verbosity=normal"