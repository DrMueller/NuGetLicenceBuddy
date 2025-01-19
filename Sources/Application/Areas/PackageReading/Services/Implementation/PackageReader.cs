using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Areas.PackageReading.Models;
using Mmu.NuGetLicenceBuddy.Areas.PackageReading.Services.Servants;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes.Implementation;
using Mmu.NuGetLicenceBuddy.Infrastructure.Logging.Services;
using Newtonsoft.Json.Linq;

namespace Mmu.NuGetLicenceBuddy.Areas.PackageReading.Services.Implementation
{
    [UsedImplicitly]
    public class PackageReader(
        ILoggingService logger,
        ITransitiveDependencyFactory transitiveDepFactory) : IPackageReader
    {
        public async Task<Maybe<IReadOnlyCollection<PackageIdentifier>>> TryReadingAsync(
            string sourcePath,
            bool includeTransitiveDependencies)
        {
            return await TryGettingAssetsJsonContentAsync(sourcePath)
                .MapAsync(CreateInternalAsync)
                .MapAsync(graph => MapIdentifiers(graph, includeTransitiveDependencies));
        }

        private static IReadOnlyCollection<PackageIdentifier> MapIdentifiers(
            DependencyGraph graph,
            bool includeTransitive)
        {
            var packages = graph.Packages.Select(f => f.Identifier).ToList();

            if (includeTransitive)
            {
                var transitiveDeps = graph.Packages.SelectMany(f => f.TransitiveDependencies)
                    .Select(f => f.PackageIdentifier);

                packages.AddRange(transitiveDeps);
            }

            return packages
                .Distinct()
                .ToList();
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

                var nuget = new NugetPackage(
                    PackageIdentifier.Parse(dep.Name),
                    transitiveDeps);

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

                return None.Value;
            }

            var content = await File.ReadAllTextAsync(assetsJsonPath);

            return content;
        }
    }
}