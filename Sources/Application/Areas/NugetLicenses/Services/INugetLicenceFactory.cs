using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByAssetsJson.Models;
using Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Services
{
    public interface INugetLicenceFactory
    {
        Task<IReadOnlyCollection<NugetLicence>> CreateAllAsync(IReadOnlyCollection<NugetPackage> packages);
    }
}
