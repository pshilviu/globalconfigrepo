# Introduction 
Nuget package for common HTTP Client functionality

# Features

* Install a typed service with a HttpClient
* Azure Ad Secured handler for making http calls to a Azure Ad secure service
  * Bearer token acquisition.

# Config requirements
``` json
"NameOfTheClassInheritedFromAzureAdClientOptions":{
    "BaseUrl": "Base url of server",
    "Timeout": "Timeout if required (defaults to 30 seconds)",
    "Scopes": "list of scopes required to get access token if you want to override from what is already in azure ad config section, space delimited"
  }
```

---
# Build Package Locally
Set current working directory to folder where .sln file is present:
* In cmd execute `dotnet pack ./`
  * This will create a nuget package in `\bin\Debug` that will allow you to work locally if required
* Alternatively you can use your IDE of choice to pack the solution using the GUI. Both Rider and Visual studio have these features
  * https://docs.microsoft.com/en-us/nuget/quickstart/create-and-publish-a-package-using-visual-studio?tabs=netcore-cli#run-the-pack-command
  * https://www.jetbrains.com/help/rider/Creating_NuGet_packages.html
