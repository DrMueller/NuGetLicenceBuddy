using Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Models;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Services.Servants
{
    public interface ILicenceFileReader
    {
        Task<Maybe<Licence>> TryReadingAsync(string licenceUrl);
    }
}