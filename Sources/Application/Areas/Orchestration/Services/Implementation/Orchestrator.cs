using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByAssetsJson.Services;
using Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Services;
using Mmu.NuGetLicenceBuddy.Areas.Outputs.Services;

namespace Mmu.NuGetLicenceBuddy.Areas.Orchestration.Services.Implementation
{
    public class Orchestrator : IOrchestrator
    {
        private readonly IDependencyGraphFactory _dependecyGraphFactory;
        private readonly IMarkdownTableFactory _markdownTableFactory;
        private readonly INugetLicenceFactory _licenceFactory;

        public Orchestrator(
            INugetLicenceFactory licenceFactory,
            IDependencyGraphFactory dependecyGraphFactory,
            IMarkdownTableFactory markdownTableFactory)
        {
            _licenceFactory = licenceFactory;
            _dependecyGraphFactory = dependecyGraphFactory;
            _markdownTableFactory = markdownTableFactory;
        }

        public async Task OrchestrateAsync(string sourcePath)
        {
            var assetsJsonPath = Directory.GetFiles(sourcePath, "project.assets.json", SearchOption.AllDirectories).FirstOrDefault();
            var content = await File.ReadAllTextAsync(assetsJsonPath);
            var dependencyGraph = await _dependecyGraphFactory.CreateFromJsonAsync(content);
            var nugetLicences = await _licenceFactory.CreateAllAsync(dependencyGraph.Packages);

            var md = _markdownTableFactory.CreateTable(nugetLicences);
            File.WriteAllText(@"C:\Users\matthias.mueller\Desktop\TMp.md", md);
            Console.WriteLine(md);
        }
    }
}