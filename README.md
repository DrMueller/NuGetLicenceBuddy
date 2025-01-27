# NuGet Licence Buddy [![NuGet](https://img.shields.io/nuget/v/nuget-license.svg)](https://www.nuget.org/packages/NuGetLicenceBuddy)

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

## Example configurations

```
nugetlicencebuddy -i -s $(Build.SourcesDirectory) -a MIT -e .*(Microsoft^|System).* -o 
$(Build.SourcesDirectory)\Sources\Application\bin\Debug -m
```

## Options

Usage: nugetlicencebuddy [options]

Option                          | Description                                                                                                                                                         
------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------
`-a\|--allowed-licences`        | Comma-seperated list, for example 'mit,apache-2'. If none is provided, all licences are allowed.                                                                    
`-e\|--exclude-packages-filter` | RegEx to exclude packages from analyzing. Must be escaped for Azure DevOps, see https://www.robvanderwoude.com/escapechars.php. Example: '.*(Microsoft^|System).*'.
`-i\|--include-transitive`      | Include distinct transitive package licenses.                                                                                                                       
`-m\|--match-output-version`    | If true, only the versions in the output folders are used. Required output-path option.                                                                             
`-o\|--output-path`             | Path of the produced artifacts.                                                                                                                                     
`-s\|--sources-path`            | Source path to search the 'project.assets.json' in.