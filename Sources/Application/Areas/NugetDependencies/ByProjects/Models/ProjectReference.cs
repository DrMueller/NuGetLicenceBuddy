using System.Xml.Linq;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByProjects.Models
{
    public class ProjectReference(string relativePath, string baseDirectory)
    {
        public string AssemblyName
        {
            get
            {
                var xDoc = XDocument.Load(AbsolutePath);

                var assemblyName = xDoc.Descendants().Where(f => f.Name == "AssemblyName")
                    .Select(f => f.Value).FirstOrDefault();

                assemblyName ??= Path.GetFileName(RelativePath).Replace(".csproj", string.Empty);

                return assemblyName;
            }
        }

        private string AbsolutePath { get; } = Path.GetFullPath(Path.Join(baseDirectory, relativePath));

        private string RelativePath { get; } = relativePath;
    }
}