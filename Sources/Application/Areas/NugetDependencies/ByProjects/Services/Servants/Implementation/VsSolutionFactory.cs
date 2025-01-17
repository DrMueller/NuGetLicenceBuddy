using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByProjects.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByProjects.Services.Servants.Implementation
{
    [UsedImplicitly]
    public class VsSolutionFactory(ICsProjectFactory csProjectFactory) : IVsSolutionFactory
    {
        public VsSolution Create(string sourcesFilePath)
        {
            var projects = GetAllCsProjFiles(sourcesFilePath)
                .Select(csProjectFactory.Create)
                .ToList();

            return new VsSolution(projects);
        }

        private static IReadOnlyCollection<string> GetAllCsProjFiles(string sourceFilePath)
        {
            return Directory
                .GetFiles(sourceFilePath, "*.csproj", SearchOption.AllDirectories)
                .Where(f => !f.Contains("Tools"))
                .ToList();
        }
    }
}