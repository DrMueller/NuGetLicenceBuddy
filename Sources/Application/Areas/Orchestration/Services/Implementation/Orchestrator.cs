using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.Services;
using Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Services;
using Mmu.NuGetLicenceBuddy.Areas.Outputs.Services;
using Mmu.NuGetLicenceBuddy.Infrastructure.Logging;

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
        public async Task OrchestrateAsync(string sourcePath)
        {
            var assetsJsonPath = Directory.GetFiles(sourcePath, "project.assets.json", SearchOption.AllDirectories).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(assetsJsonPath))
            {
                logger.LogError("project.assets.json file not found. Cancelling..");
                return;
            }

            var content = await File.ReadAllTextAsync(assetsJsonPath);
            var dependencyGraph = await dependecyGraphFactory.CreateFromJsonAsync(content);

            // TODO option for transitive
            var packages = dependencyGraph.Packages.Select(f => f.Identifier).Distinct().ToList();
            var nugetLicences = await licenceFactory.CreateAllAsync(packages);

            var md = markdownTableFactory.CreateTable(nugetLicences);
            await File.WriteAllTextAsync(@"C:\Users\matthias.mueller\Desktop\TMp.md", md);
            Console.WriteLine(md);
        }
    }
}