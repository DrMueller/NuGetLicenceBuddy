using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mmu.NuGetLicenceBuddy.Areas.Outputs.Services
{
    public interface IOutputWriter
    {
        Task WriteToFileAsync(string text);
    }
}
