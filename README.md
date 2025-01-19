﻿# NuGet Licence Buddy [![NuGet](https://img.shields.io/nuget/v/nuget-license.svg)](https://www.nuget.org/packages/NuGetLicenceBuddy)

A .NET tool to print and validate the licences of .NET code.  
This tool supports all versions that can produce a `project.assets.json` file.  
It is designed to be used via Azure DevOps, so the output includes Azure DevOps-specific prefixes to format it accordingly.

## Usage in Azure DevOps

The easiest way to use it is by combining it with the global .NET tool installer:

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

| Option | Description |
| ------ | ------------------------- |
| `-a\|--allowed-licences` | Comma-seperated list, for example 'mit,apache-2'. If none is provided, all licences are allowed. |
| `-i\|--include-transitive` | Include distinct transitive package licenses. |
| `-s\|--sources-path` | Source path to search the 'project.assets.json' in. |
