using Mmu.NuGetLicenceBuddy.Areas.Options.Models;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes;

namespace Mmu.NuGetLicenceBuddy.Areas.Options.Services
{
    public interface IOptionsFactory
    {
        Maybe<ToolOptions> TryCreating(string[] args);
    }
}