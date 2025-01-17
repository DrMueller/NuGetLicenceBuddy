using JetBrains.Annotations;
using Lamar;
using Microsoft.Extensions.DependencyInjection;

namespace Mmu.NuGetLicenceBuddy.Infrastructure.DependencyInjection
{
    [UsedImplicitly]
    public class ServiceRegistryCollection : ServiceRegistry
    {
        public ServiceRegistryCollection()
        {
            Scan(scanner =>
            {
                scanner.AssemblyContainingType<ServiceRegistryCollection>();
                scanner.WithDefaultConventions();
            });

            this.AddHttpClient();
        }
    }
}