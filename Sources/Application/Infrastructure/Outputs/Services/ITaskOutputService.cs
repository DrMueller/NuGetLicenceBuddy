namespace Mmu.NuGetLicenceBuddy.Infrastructure.Outputs.Services
{
    public interface ITaskOutputService
    {
        void FailTask();
        void SucceedTask();
    }
}