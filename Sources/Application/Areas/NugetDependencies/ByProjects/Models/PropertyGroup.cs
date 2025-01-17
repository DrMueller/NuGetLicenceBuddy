namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByProjects.Models
{
    public class PropertyGroup(
        bool nullableEnabled,
        bool generateAssemblyInfo,
        bool allowUnsafeBlocks)
    {
        public bool AllowUnsafeBlocks { get; } = allowUnsafeBlocks;
        public bool GenerateAssemblyInfo { get; } = generateAssemblyInfo;
        public bool NullableEnabled { get; } = nullableEnabled;

        public static PropertyGroup CreateEmpty()
        {
            return new PropertyGroup(false, false, false);
        }
    }
}