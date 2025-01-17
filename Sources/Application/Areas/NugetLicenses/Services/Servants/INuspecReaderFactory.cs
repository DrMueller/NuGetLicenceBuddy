using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByAssetsJson.Models;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes;
using NuGet.Packaging;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Services.Servants
{
    public interface INuspecReaderFactory
    {
        Task<IReadOnlyCollection<NuspecReader>> CreateAllAsync(IReadOnlyCollection<NugetPackage> packages);
    }
}
