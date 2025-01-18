using CommandLine;
using JetBrains.Annotations;

namespace Mmu.NuGetLicenceBuddy.Infrastructure.Options.Models
{
    [PublicAPI]
    public class ToolOptions
    {
        [Option('a', "allowed-licences", HelpText = "Comma-seperated list, for example 'mit,apache-2'. If none is provided, all licences are allowed.", Required = false)]
        public string AllowedLicences { get; set; } = default!;

        [Option('i', "include-transitive", Default = false, HelpText = "Include distinct transitive package licenses.")]
        public bool IncludeTransitiveDependencies { get; set; }

        [Option('s', "sources-path", HelpText = "Source path to search the 'project.assets.json' in.", Required = true)]
        public string SourcesPath { get; set; } = default!;
    }
}