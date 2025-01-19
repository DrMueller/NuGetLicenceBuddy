namespace Mmu.NuGetLicenceBuddy.Infrastructure.Outputs.Services.Implementation
{
    public class TaskFailer : ITaskFailer
    {
        public void FailTask()
        {
            Console.WriteLine("##vso[task.complete result=Failed;]Task failed due to an error.");
            Environment.Exit(1);
        }
    }
}