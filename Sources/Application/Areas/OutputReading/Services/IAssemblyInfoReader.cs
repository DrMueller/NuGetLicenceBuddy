using Mmu.NuGetLicenceBuddy.Areas.OutputReading.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.OutputReading.Services
{
    public interface IAssemblyInfoReader
    {
        Task<IReadOnlyCollection<AssemblyInfo>> ReadAllAsync(string outputPath);
    }
}