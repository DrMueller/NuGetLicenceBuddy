using System.Text.RegularExpressions;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Invariance;

namespace Mmu.NuGetLicenceBuddy.Areas.PackageReading.Models
{
    public class NugetPackages
    {
        private readonly string? _excludeFilter;

        public IReadOnlyCollection<NugetPackage> FlatPackages
        {
            get
            {
                var packages = FilteredPackages.ToList();

                if (IncludeTransitiveDependencies)
                {
                    var transitiveDeps = packages
                        .SelectMany(f => f.TransitiveDependencies)
                        .Select(f => f.PackageIdentifier)
                        .ToList();

                    var transitivePackages =
                        FilteredPackages.Where(f => transitiveDeps.Contains(f.Identifier))
                            .ToList();

                    packages.AddRange(transitivePackages);
                }

                return packages
                    .Distinct()
                    .ToList();
            }
        }

        public bool IncludeTransitiveDependencies { get; }
        public IReadOnlyCollection<NugetPackage> Values { get; }

        private IReadOnlyCollection<NugetPackage> FilteredPackages
        {
            get
            {
                if (string.IsNullOrEmpty(_excludeFilter))
                {
                    return Values;
                }

                var regex = new Regex(_excludeFilter);

                return
                    Values
                        .Where(f => !regex.IsMatch(f.Identifier.PackageName))
                        .ToList();
            }
        }

        public NugetPackages(
            IReadOnlyCollection<NugetPackage> values,
            bool includeTransitiveDependencies,
            string? excludeFilter)
        {
            Guard.CollectionNotNullOrEmpty(() => values);

            _excludeFilter = excludeFilter;
            Values = values;
            IncludeTransitiveDependencies = includeTransitiveDependencies;
        }
    }
}