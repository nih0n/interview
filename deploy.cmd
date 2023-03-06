@echo off

set ASPNETCORE_ENVIRONMENT=Production

dotnet publish ./src/API/Solution.API.csproj --configuration Release --runtime win-x64 --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=true -o ./publish

./publish/Solution.API.exe