namespace Mmu.NuGetLicenceBuddy.Infrastructure.Outputs.Services
{
    public interface IOutputWriter
    {
        Task WriteToFileAsync(string text);
    }
}