#!/bin/bash
dotnet test Cafe.Tests.csproj -p:Exclude=[xunit*]* -p:CollectCoverage=true -p:CoverletOutputFormat=opencover --logger:"console;verbosity=normal"