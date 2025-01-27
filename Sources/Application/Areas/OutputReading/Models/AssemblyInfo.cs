namespace Mmu.NuGetLicenceBuddy.Areas.OutputReading.Models
{
    public record AssemblyInfo(string AssemblyName, string AssemblyVersion)
    {
        public bool IsMatch(string assemblyName, string assemblyVersion)
        {
            return AssemblyName == assemblyName && AssemblyVersion == assemblyVersion;
        }
    }
}