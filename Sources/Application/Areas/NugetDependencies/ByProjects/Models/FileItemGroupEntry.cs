namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByProjects.Models
{
    public class FileItemGroupEntry
    {
        public FileItemGroupEntry(
            string buildAction,
            string buildContext,
            string filePath)
        {
            BuildAction = buildAction;
            BuildContext = buildContext;
            FilePath = filePath;
        }

        public string BuildAction { get; }
        public string BuildContext { get; }
        public string FilePath { get; }
    }
}