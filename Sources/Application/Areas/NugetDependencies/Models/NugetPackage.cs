using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Invariance;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.Models
{
    [PublicAPI]
    public class NugetPackage
    {
        public IReadOnlyCollection<NugetPackage> DependsOn { get; private set; }
        public PackageIdentifier Identifier { get; }
        public IReadOnlyCollection<TransitiveDependency> TransitiveDependencies { get; }

        public NugetPackage(
            PackageIdentifier identifier,
            IReadOnlyCollection<TransitiveDependency> transitiveDependencies)
        {
            Guard.ObjectNotNull(() => identifier);
            Guard.ObjectNotNull(() => transitiveDependencies);

            DependsOn = new List<NugetPackage>();
            Identifier = identifier;
            TransitiveDependencies = transitiveDependencies;
        }

        internal void UpdateDependsOn(IReadOnlyCollection<NugetPackage> dependsOn)
        {
            DependsOn = dependsOn;
        }
    }
}