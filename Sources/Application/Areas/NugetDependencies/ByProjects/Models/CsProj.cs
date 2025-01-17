using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.Common.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByProjects.Models
{
    public class CsProj(
        string assemblyPath,
        string assemblyName,
        string targetFramework,
        IReadOnlyCollection<NugetPackageReference> nugetReferences,
        IReadOnlyCollection<ProjectReference> projectReferences,
        PropertyGroup propertyGroup,
        EmbeddedResources embeddedResources,
        IReadOnlyCollection<FileItemGroup> fileItemGroups)
    {
        public string AssemblyName { get; } = assemblyName;
        public string AssemblyPath { get; } = assemblyPath;
        public EmbeddedResources EmbeddedResources { get; } = embeddedResources;
        public IReadOnlyCollection<FileItemGroup> FileItemGroups { get; } = fileItemGroups;

        public bool IsTestProject => AssemblyName.EndsWith("Tests", StringComparison.OrdinalIgnoreCase);
        public IReadOnlyCollection<NugetPackageReference> NugetReferences { get; } = nugetReferences;
        public IReadOnlyCollection<ProjectReference> ProjectReferences { get; } = projectReferences;
        public PropertyGroup PropertyGroup { get; } = propertyGroup;
        public string TargetFramework { get; } = targetFramework;

        public bool ContainNugetsReferences(IReadOnlyCollection<string> nugetReferenceNames)
        {
            var actualNugetReferenceNames = NugetReferences.Select(f => f.PackageName).ToList();

            return nugetReferenceNames.All(refName => actualNugetReferenceNames.Contains(refName));
        }
    }
}