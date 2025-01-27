using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Invariance;

namespace Mmu.NuGetLicenceBuddy.Areas.PackageReading.Models
{
    [PublicAPI]
    public class NugetPackage
    {
        public IReadOnlyCollection<NugetPackage> DependsOn { get; private set; }
        public string DllName { get; }
        public PackageIdentifier Identifier { get; }
        public IReadOnlyCollection<TransitiveDependency> TransitiveDependencies { get; }

        public NugetPackage(
            PackageIdentifier identifier,
            IReadOnlyCollection<TransitiveDependency> transitiveDependencies,
            string dllName)
        {
            Guard.ObjectNotNull(() => identifier);
            Guard.ObjectNotNull(() => transitiveDependencies);
            Guard.StringNotNullOrEmpty(() => dllName);

            DependsOn = new List<NugetPackage>();
            Identifier = identifier;
            TransitiveDependencies = transitiveDependencies;
            DllName = dllName;
        }

        internal void UpdateDependsOn(IReadOnlyCollection<NugetPackage> dependsOn)
        {
            DependsOn = dependsOn;
        }
    }
}