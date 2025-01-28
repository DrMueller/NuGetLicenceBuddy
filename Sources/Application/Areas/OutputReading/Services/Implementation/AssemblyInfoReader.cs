using System.Diagnostics;
using System.IO.Abstractions;
using System.Reflection;
using Mmu.NuGetLicenceBuddy.Areas.OutputReading.Models;
using Mmu.NuGetLicenceBuddy.Infrastructure.Logging.Services;

namespace Mmu.NuGetLicenceBuddy.Areas.OutputReading.Services.Implementation
{
    public class AssemblyInfoReader(
        ILoggingService logger,
        IFileSystem fileSystem) : IAssemblyInfoReader
    {
        public Task<IReadOnlyCollection<AssemblyInfo>> ReadAllAsync(string outputPath)
        {
            var result = new List<AssemblyInfo>();
            var allDlls = fileSystem.Directory.GetFiles(outputPath, "*.dll", SearchOption.AllDirectories);

            foreach (var dllPath in allDlls)
            {
                try
                {
                    var fileInfo = new FileInfo(dllPath);
                    var assembly = Assembly.LoadFile(fileInfo.FullName);
                    var assemblyName = assembly.GetName();
                    var info = FileVersionInfo.GetVersionInfo(fileInfo.FullName);

                    var version = $"{info.FileMajorPart}.{info.FileMinorPart}.{info.FileBuildPart}";
                    result.Add(new AssemblyInfo(assemblyName.Name!, version));
                }
                catch (Exception ex)
                {
                    logger.LogDebug($"Error reading assembly info from {dllPath}: {ex.Message}");
                }
            }

            foreach (var item in result)
            {
                logger.LogDebug($"{item.AssemblyName} {item.AssemblyVersion}");
            }

            return Task.FromResult<IReadOnlyCollection<AssemblyInfo>>(result);
        }
    }
}