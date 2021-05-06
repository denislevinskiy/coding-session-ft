# Coding Session: Integration/fault-tolerance patterns

A repo created specially for coding session "Integration/fault-tolerance patterns". Includes custom implementation
of **Retry** and **Circuit Breaker** patterns, adapted to HTTP REST Api as well, as the equal examples based on **Polly** library.

Executables:
* **CodingSessionFT.Api** - fake REST API service based on .NET Core 3.1
* **CodingSessionFT.Client** - console API client based on .NET Core 3.1 with custom implementation of presented patterns
* **CodingSessionFT.PollyClient** - console API client based on .NET Core 3.1 with implementation of presented patterns based on **Polly**

## Getting Started

Clone or download the repo and go to project root folder.

### Build the project

For Windows system run `build.ps1` or `build.cmd` from command line. For Linux run `build.sh`.

### Run the project

**Run fake Api**

```
dotnet run --project .\src\CodingSessionFT.Api\CodingSessionFT.Api.csproj`
```

## Built With

* [.NET Core 3.1](https://docs.microsoft.com/en-us/dotnet/core/) - The primary framework
* [Nuke.Build](https://nuke.build/) - The primary build framework
* [Polly](https://github.com/App-vNext/Polly) - Http client helpers library