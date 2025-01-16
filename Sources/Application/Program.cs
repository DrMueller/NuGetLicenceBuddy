using System.Reflection;

namespace Mmu.NuGetLicenceBuddy
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var versionString = Assembly.GetEntryAssembly()?
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                .InformationalVersion;

            Console.WriteLine($"NuGetLicenseBuddy v{versionString}");
            Console.WriteLine("-------------");
            Console.WriteLine("\nUsage:");
            Console.WriteLine("  botsay <message>");
        }
    }
}