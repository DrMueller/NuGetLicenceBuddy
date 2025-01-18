using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes;
using Mmu.NuGetLicenceBuddy.Infrastructure.Options.Models;

namespace Mmu.NuGetLicenceBuddy.Infrastructure.Options.Services
{
    public interface IOptionsFactory
    {
        Maybe<ToolOptions> TryCreating(string[] args);
    }
}