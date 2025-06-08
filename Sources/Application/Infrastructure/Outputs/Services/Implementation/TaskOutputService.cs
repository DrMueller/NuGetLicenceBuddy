namespace Mmu.NuGetLicenceBuddy.Infrastructure.Outputs.Services.Implementation
{
    public class TaskOutputService : ITaskOutputService
    {
        public void FailTask()
        {
            Console.Error.WriteLine("##vso[task.complete result=Failed;]Task failed due to an error.");

            Console.Error.Flush();

            Environment.Exit(1);
        }

        public void SucceedTask()
        {
            Environment.Exit(0);
        }
    }
}