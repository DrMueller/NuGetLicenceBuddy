using Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.Outputs.Services
{
    public interface IMarkdownTableFactory
    {
        string CreateTable(IReadOnlyCollection<NugetLicence> licences);
    }
}