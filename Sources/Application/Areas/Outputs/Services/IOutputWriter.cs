namespace Mmu.NuGetLicenceBuddy.Areas.Outputs.Services
{
    public interface IOutputWriter
    {
        Task WriteToFileAsync(string text);
    }
}