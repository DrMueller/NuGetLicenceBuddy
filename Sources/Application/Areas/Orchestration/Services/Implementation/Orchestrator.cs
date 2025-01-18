using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.Models;
using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.Services;
using Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Services;
using Mmu.NuGetLicenceBuddy.Areas.Outputs.Services;
using Mmu.NuGetLicenceBuddy.Infrastructure.Logging;
using Mmu.NuGetLicenceBuddy.Infrastructure.Options.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.Orchestration.Services.Implementation
{
    [UsedImplicitly]
    public class Orchestrator(
        INugetLicenceFactory licenceFactory,
        IDependencyGraphFactory dependecyGraphFactory,
        IMarkdownTableFactory markdownTableFactory,
        ILoggingService logger)
        : IOrchestrator
    {
        public async Task OrchestrateAsync(ToolOptions options)
        {
            var replacedSourcesPath = options.SourcesPath.Replace(@"\", @"\\");
            var assetsJsonPath = Directory
                .GetFiles(
                    replacedSourcesPath,
                    "project.assets.json",
                    SearchOption.AllDirectories).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(assetsJsonPath))
            {
                logger.LogError("project.assets.json file not found. Cancelling..");

                return;
            }

            var content = await File.ReadAllTextAsync(assetsJsonPath);
            var dependencyGraph = await dependecyGraphFactory.CreateFromJsonAsync(content);
            var packages = Map(dependencyGraph, options.IncludeTransitiveDependencies);
            var nugetLicences = await licenceFactory.CreateAllAsync(packages);

            var md = markdownTableFactory.CreateTable(nugetLicences);
            await File.WriteAllTextAsync(@"C:\Users\matthias.mueller\Desktop\TMp.md", md);
            Console.WriteLine(md);
        }

        private static IReadOnlyCollection<PackageIdentifier> Map(DependencyGraph graph, bool includeTransitive)
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
    }
}