using System.IO.Abstractions;
using Mmu.NuGetLicenceBuddy.Infrastructure.Logging.Services;

namespace Mmu.NuGetLicenceBuddy.Infrastructure.Outputs.Services.Implementation
{
    public class OutputWriter(
        ILoggingService logger,
        IFileSystem fileSystem) : IOutputWriter
    {
        public async Task WriteToFileAsync(string text)
        {
            var outputPath = Environment.CurrentDirectory;
            var outputFilePath = fileSystem.Path.Combine(outputPath, "licences.txt");

            logger.LogInfo($"Writing output to {outputFilePath}..");

            if (!fileSystem.Directory.Exists(outputPath))
            {
                fileSystem.Directory.CreateDirectory(outputPath);
            }

            await fileSystem.File.WriteAllTextAsync(outputFilePath, text);
        }
    }
}