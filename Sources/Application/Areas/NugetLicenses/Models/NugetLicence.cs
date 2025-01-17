namespace Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Models
{
    public record NugetLicence(
        string NugetIdentifier,
        string NugetVersion,
        string NugetLicenceUrl,
        Licence Licence);
}