using JetBrains.Annotations;
using Lamar;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Abstractions;

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

            For<IFileSystem>().Use<FileSystem>().Scoped();
            this.AddHttpClient();
        }
    }
}