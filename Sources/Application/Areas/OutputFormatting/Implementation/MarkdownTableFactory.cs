using System.Text;
using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.OutputFormatting.Implementation
{
    [UsedImplicitly]
    public class MarkdownTableFactory : IMarkdownTableFactory
    {
        public string CreateTable(IReadOnlyCollection<NugetLicence> licences)
        {
            var sb = new StringBuilder();
            sb.AppendLine("|NuGet Name|NuGet Version|Licence Name|License URL|");
            sb.AppendLine("|-----------------|-----------------|-----------------|-------------------|");

            foreach (var licence in licences)
            {
                sb.AppendLine($"|{licence.NugetIdentifier}|{licence.NugetVersion}|{licence.Licence.Name}|{licence.NugetLicenceUrl}|");
            }

            return sb.ToString();
        }
    }
}