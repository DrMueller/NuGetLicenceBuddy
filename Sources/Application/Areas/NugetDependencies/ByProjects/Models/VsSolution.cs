using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.Common.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByProjects.Models
{
    public class VsSolution(IReadOnlyCollection<CsProj> projects)
    {
        public IReadOnlyCollection<CsProj> Projects { get; } = projects;

        public IReadOnlyCollection<CsProj> TestProjects => Projects.Where(f => f.IsTestProject).ToList();

        private IReadOnlyCollection<NugetPackageReference> DuplicatedNugetReferences
        {
            get
            {
                var namespacesToIgnore = new
                    List<string>
                    {
                        "Analyzers",
                        "CodeAnalysis",
                        "Microsoft.NET.Test.Sdk"
                    };

                return Projects
                    .SelectMany(f => f.NugetReferences)
                    .Where(f => namespacesToIgnore.All(ns => !f.PackageName.Contains(ns, StringComparison.OrdinalIgnoreCase)))
                    .GroupBy(f => f)
                    .Where(f => f.Count() > 1)
                    .SelectMany(f => f)
                    .ToList();
            }
        }

        public IReadOnlyCollection<NugetPackageReference> CalculateDuplicatedNugets()
        {
            var actualDuplicates = new List<NugetPackageReference>();

            foreach (var dup in DuplicatedNugetReferences)
            {
                var projsWithDuplicate = Projects
                    .Where(f => f.NugetReferences.Contains(dup))
                    .ToList();

                foreach (var proj in projsWithDuplicate)
                {
                    var dependantProjects = new List<CsProj>();
                    GatherDependencies(proj, dependantProjects);

                    if (dependantProjects.Count(f => f.NugetReferences.Contains(dup)) > 1)
                    {
                        actualDuplicates.Add(dup);
                    }
                }
            }

            var distinctDuplicates = actualDuplicates
                .Distinct()
                .ToList();

            return distinctDuplicates;
        }

        private void GatherDependencies(CsProj proj, ICollection<CsProj> references)
        {
            if (!references.Contains(proj))
            {
                references.Add(proj);
            }

            foreach (var refProj in proj.ProjectReferences)
            {
                var childProj = Projects.Single(f => f.AssemblyName == refProj.AssemblyName);
                GatherDependencies(childProj, references);
            }
        }
    }
}