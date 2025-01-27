namespace Mmu.NuGetLicenceBuddy.Areas.LicenceFetching.Models
{
    public record NugetLicence(
        string NugetIdentifier,
        string NugetVersion,
        string NugetLicenceUrl,
        string NugetDllName,
        Licence Licence);
}