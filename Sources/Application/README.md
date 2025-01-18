# NuGet Licence Buddy [![NuGet](https://img.shields.io/nuget/v/nuget-license.svg)](https://www.nuget.org/packages/NuGetLicenceBuddy)

A .net tool to print and validate the licences of .net code. 
This tool supports all versions, which can produce an 'project.assets.json' file.
This tool is meant to be used via Azure DevOps, therefore the output has some Azure DevOps releated prefixes to format it accordingly.

## Using in Azure DevOps

It's the easiest to combine it with the global dotnet-tool installer:

```yaml
pool:
  name: Azure Pipelines
steps:
- task: petersendev.dotnet-global-tool-installer.DotnetGlobalToolInstaller.DotnetGlobalToolInstaller@0
  displayName: '.NET Core Global Tool'
  inputs:
    name: nugetlicencebuddy
    versionSpec: 1.0.17

- script: 'nugetlicencebuddy -t true -s $(Build.SourcesDirectory)'
  displayName: 'Run Licence Buddy'
```

## Options

Usage: nugetlicencebuddy [options]

**Options:**

| Option | Description |
| ------ | ------------------------- |
|`-s\|--sources-path <INPUT_PATH>`|Include transitive package licences.|
|`-i\|--include-transitive`|Source path to search the 'project.assets.json' in.|
|`-a\|--alloced-licences <ALLOWED_LICENSES>`|Comma-seperated list for allowed licences, for example 'mit,apache-2'. If none is provided, all licences are allowed.|