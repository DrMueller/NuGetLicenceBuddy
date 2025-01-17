using System.Xml.Linq;
using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByProjects.Models;
using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByProjects.Services.Servants;
using Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.Common.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetDependencies.ByProjects.Services.Servants.Implementation
{
    [UsedImplicitly]
    public class CsProjectFactory : ICsProjectFactory
    {
        public CsProj Create(string filePath)
        {
            var xDoc = XDocument.Load(filePath);
            var packageReferences = xDoc.Descendants().Where(f => f.Name == "PackageReference")
                .Select(f => new NugetPackageReference(
                    f.Attribute("Include")!.Value, f.Attribute("Version")!.Value))
                .ToList();

            var projectReferences = xDoc.Descendants().Where(f => f.Name == "ProjectReference")
                .Select(f => new ProjectReference(f.Attribute("Include")!.Value, Path.GetDirectoryName(filePath)!))
                .ToList();

            var assemblyName = xDoc.Descendants().Where(f => f.Name == "AssemblyName")
                .Select(f => f.Value).FirstOrDefault();

            var targetFramework = xDoc.Descendants().Where(f => f.Name == "TargetFramework")
                .Select(f => f.Value).Single();

            assemblyName ??= Path.GetFileName(filePath).Replace(".csproj", string.Empty);

            return new CsProj(
                Path.GetDirectoryName(filePath)!,
                assemblyName,
                targetFramework,
                packageReferences,
                projectReferences,
                CreatePropertyGroup(xDoc),
                CreateEmbeddedResources(xDoc),
                CreateFileItemGroups(xDoc));
        }

        private static EmbeddedResources CreateEmbeddedResources(XDocument xDoc)
        {
            var resources = xDoc
                .Descendants()
                .Where(f => f.Name == "EmbeddedResource")
                .Select(x => new EmbeddedResource(x.Attribute("Update")?.Value ?? string.Empty, x.Descendants("DependentUpon").SingleOrDefault()?.Value ?? string.Empty, x.Descendants("Generator").SingleOrDefault()?.Value ?? string.Empty))
                .ToList();

            return new EmbeddedResources(resources);
        }

        private static IReadOnlyCollection<FileItemGroup> CreateFileItemGroups(XDocument xDoc)
        {
            var result = new List<FileItemGroup>();
            var itemGroups = xDoc.Descendants("ItemGroup");

            foreach (var itemGroup in itemGroups)
            {
                // We manage these itemgroups with another object
                if (itemGroup.Elements().Any(f => f.Name == "PackageReference" || f.Name == "ProjectReference"))
                {
                    continue;
                }

                var entries = new List<FileItemGroupEntry>();
                foreach (var childElement in itemGroup.Elements())
                {
                    var attr = childElement.Attributes().Single();
                    entries.Add(new FileItemGroupEntry(childElement.Name.LocalName, attr.Name.LocalName, attr.Value));
                }

                result.Add(new FileItemGroup(entries));
            }

            return result;
        }

        private static PropertyGroup CreatePropertyGroup(XContainer xDoc)
        {
            var propGroupDoc = xDoc.Descendants().SingleOrDefault(f => f.Name == "PropertyGroup");
            if (propGroupDoc == null)
            {
                return PropertyGroup.CreateEmpty();
            }

            var nullableEnabled = propGroupDoc.Descendants().SingleOrDefault(f => f.Name == "Nullable")?.Value == "enable";
            var allowUnsafeBlocks = propGroupDoc.Descendants().SingleOrDefault(f => f.Name == "AllowUnsafeBlocks")?.Value == "true";
            var generateAssemblyInfoConfig = propGroupDoc.Descendants().SingleOrDefault(f => f.Name == "GenerateAssemblyInfo")?.Value;
            var generateAssemblyInfo = string.IsNullOrEmpty(generateAssemblyInfoConfig) || bool.Parse(generateAssemblyInfoConfig);

            return new PropertyGroup(nullableEnabled, generateAssemblyInfo, allowUnsafeBlocks);
        }
    }
}