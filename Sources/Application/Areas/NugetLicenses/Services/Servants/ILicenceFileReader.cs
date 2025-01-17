using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Models;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Services.Servants
{
    public interface ILicenceFileReader 
    {
        Task<Maybe<Licence>> TryReadingAsync(string licenceUrl);
    }
}
