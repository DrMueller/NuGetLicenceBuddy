using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByAssetsJson.Models;
using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByAssetsJson.Services.Servants;
using Newtonsoft.Json.Linq;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByAssetsJson.Services.Implementation
{
    public class DependencyGraphFactory : IDependencyGraphFactory
    {
        private readonly ITransitiveDependencyFactory _transitiveDepFactory;

        public DependencyGraphFactory(
            ITransitiveDependencyFactory transitiveDepFactory)
        {
            _transitiveDepFactory = transitiveDepFactory;
        }

        public async Task<DependencyGraph> CreateFromJsonAsync(string json)
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

                var transitiveDeps = await _transitiveDepFactory.CreateAsync(target.Name, new NugetIdentifier(dep.Name));

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
    }
}