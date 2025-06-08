using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Areas.PackageReading.Models;
using Mmu.NuGetLicenceBuddy.Areas.PackageReading.Services.Servants;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes.Implementation;
using Mmu.NuGetLicenceBuddy.Infrastructure.Logging.Services;
using Mmu.NuGetLicenceBuddy.Infrastructure.Outputs.Services;
using Newtonsoft.Json.Linq;

namespace Mmu.NuGetLicenceBuddy.Areas.PackageReading.Services.Implementation
{
    [UsedImplicitly]
    public class PackageReader(
        ILoggingService logger,
        ITransitiveDependencyFactory transitiveDepFactory,
        ITaskOutputService outputService) : IPackageReader
    {
        public async Task<Maybe<NugetPackages>> TryReadingAsync(
            string sourcePath,
            bool includeTransitiveDependencies,
            string? excludePackagesFilterOption)
        {
            return await TryGettingAssetsJsonContentAsync(sourcePath)
                .MapAsync(CreateInternalAsync)
                .MapAsync(f => new NugetPackages(
                    f.Packages,
                    includeTransitiveDependencies,
                    excludePackagesFilterOption));
        }

        private async Task<DependencyGraph> CreateInternalAsync(string json)
        {
            var root = JObject.Parse(json);
            var target = root.Properties().Single(f => f.Name == "targets")
                .Cast<JObject>()
                .Properties()
                .Single();

            var netObj = (JObject)target.Value;

            var netObjDeps = netObj.Properties().Where(f => f.Value.Type == JTokenType.Object)
                .ToList();

            var allNugets = new List<(NugetPackage, List<PackageDependency>)>();

            // This is a flat list
            foreach (var dep in netObjDeps)
            {
                var dependencyPackages = new List<PackageDependency>();
                var val = (JObject)dep.Value;
                var depDependencies = val.Properties()
                    .SingleOrDefault(f => f.Name == "dependencies");

                if (depDependencies != null)
                {
                    var devProps = (JObject)depDependencies.Value;

                    foreach (var dd in devProps.Properties())
                    {
                        var packageDep = new PackageDependency(new PackageIdentifier(dd.Name, dd.Value.ToString()));
                        dependencyPackages.Add(packageDep);
                    }
                }

                var transitiveDeps = await transitiveDepFactory.CreateAsync(target.Name, new NugetIdentifier(dep.Name));

                var compileToken = (JProperty?)dep.Value
                    .SelectToken("compile")?
                    .FirstOrDefault();

                var dllName = compileToken?.Name.Split('/').Last();

                var nuget = new NugetPackage(
                    PackageIdentifier.Parse(dep.Name),
                    transitiveDeps,
                    dllName ?? "(Not found)");

                allNugets.Add((nuget, dependencyPackages));
            }

            var plainNugetList = allNugets.Select(f => f.Item1).ToList();

            foreach (var nuget in allNugets)
            {
                // Some packages are not in the flat list, but are added as dependencies
                // I didn't find out what this means
                var dependsOn = nuget.Item2.Select(f => { return plainNugetList.SingleOrDefault(p => p.Identifier == f.Identifier); })
                    .Where(f => f != null)
                    .ToList();

                nuget.Item1.UpdateDependsOn(dependsOn!);
            }

            return new DependencyGraph(target.Name, plainNugetList);
        }

        private async Task<Maybe<string>> TryGettingAssetsJsonContentAsync(string sourceFilePath)
        {
            var replacedSourcesPath = sourceFilePath.Replace(@"\", @"\\");
            var assetsJsonPath = Directory
                .GetFiles(
                    replacedSourcesPath,
                    "project.assets.json",
                    SearchOption.AllDirectories).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(assetsJsonPath))
            {
                logger.LogError("project.assets.json file not found. Cancelling..");

                outputService.FailTask();

                return None.Value;
            }

            var content = await File.ReadAllTextAsync(assetsJsonPath);

            return content;
        }
    }
}