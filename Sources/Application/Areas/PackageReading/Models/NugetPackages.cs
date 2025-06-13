using System.Text.RegularExpressions;

namespace Mmu.NuGetLicenceBuddy.Areas.PackageReading.Models
{
    public class NugetPackages(
        IReadOnlyCollection<NugetPackage> values,
        bool includeTransitiveDependencies,
        string? excludeFilter)
    {
        public IReadOnlyCollection<NugetPackage> FlatPackages
        {
            get
            {
                var packages = FilteredPackages.ToList();

                if (!IncludeTransitiveDependencies)
                {
                    return packages
                        .Distinct()
                        .ToList();
                }

                var transitiveDeps = packages
                    .SelectMany(f => f.TransitiveDependencies)
                    .Select(f => f.PackageIdentifier)
                    .ToList();

                var transitivePackages =
                    FilteredPackages.Where(f => transitiveDeps.Contains(f.Identifier))
                        .ToList();

                packages.AddRange(transitivePackages);

                return packages
                    .Distinct()
                    .ToList();
            }
        }

        public bool IncludeTransitiveDependencies { get; } = includeTransitiveDependencies;
        public IReadOnlyCollection<NugetPackage> Values { get; } = values;

        private IReadOnlyCollection<NugetPackage> FilteredPackages
        {
            get
            {
                if (string.IsNullOrEmpty(excludeFilter))
                {
                    return Values;
                }

                var regex = new Regex(excludeFilter);

                return Values
                    .Where(f => !regex.IsMatch(f.Identifier.PackageName))
                    .ToList();
            }
        }
    }
}