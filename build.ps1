# Restore nuget packages
#dotnet restore .\ServiceDesk.sln

#dotnet build .\ServiceDesk.sln --no-restore

dotnet test .\ServiceDesk.sln --no-build --logger:"console;verbosity=detailed"

