using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Areas.LicenceFetching.Models;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions;

namespace Mmu.NuGetLicenceBuddy.Areas.OutputFormatting.Implementation
{
    [UsedImplicitly]
    public class MarkdownTableFactory : IMarkdownTableFactory
    {
        public string CreateTable(IReadOnlyCollection<NugetLicence> licences)
        {
            var table = licences
                .Select(f => new
                {
                    NugetName = f.NugetIdentifier,
                    f.NugetVersion,
                    LicenceName = f.Licence.Name,
                    LicenceUrl = f.NugetLicenceUrl
                }).ToList();

            return table.ToMarkdownTable();
        }
    }
}