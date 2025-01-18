using System.IO.Abstractions;

namespace Mmu.NuGetLicenceBuddy.Areas.Outputs.Services.Implementation
{
    public class OutputWriter(IFileSystem fileSystem) : IOutputWriter
    {
        public async Task WriteToFileAsync(string text)
        {
            var outputPath = Environment.CurrentDirectory;
            var outputFilePath = fileSystem.Path.Combine(outputPath, "licences.txt");

            await fileSystem.File.WriteAllTextAsync(outputFilePath, text);
        }
    }
}