using Lamar;

namespace Mmu.NuGetLicenceBuddy.Infrastructure.DependencyInjection
{
    internal static class ContainerFactory
    {
        internal static IContainer Create()
        {
            return new Container(cfg =>
            {
                cfg.Scan(scanner =>
                {
                    scanner.AssembliesFromApplicationBaseDirectory();
                    scanner.LookForRegistries();
                });
            });
        }
    }
}