using Mmu.NuGetLicenceBuddy.Areas.LicenceFetching.Models;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes;

namespace Mmu.NuGetLicenceBuddy.Areas.LicenceFetching.Services.Servants
{
    public interface ILicenceFileReader
    {
        Task<Maybe<Licence>> TryReadingAsync(string licenceUrl);
    }
}