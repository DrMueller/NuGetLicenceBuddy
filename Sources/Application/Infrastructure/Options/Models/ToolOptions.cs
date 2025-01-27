using CommandLine;
using JetBrains.Annotations;

namespace Mmu.NuGetLicenceBuddy.Infrastructure.Options.Models
{
    [PublicAPI]
    public class ToolOptions
    {
        [Option('a', "allowed-licences", HelpText = "Comma-seperated list, for example 'mit,apache-2'. If none is provided, all licences are allowed.", Required = false)]
        public string AllowedLicences { get; set; } = default!;

        [Option('e', "exclude-packages-filter", Default = null, HelpText = "RegEx to exclude packages from analyzing. Example: '.*(Microsoft|System).*\n'")]
        public string? ExcludePackagesFilterOption { get; set; }

        [Option('i', "include-transitive", Default = false, HelpText = "Include distinct transitive package licenses.")]
        public bool IncludeTransitiveDependencies { get; set; }

        [Option('m', "match-output-version", Default = false, HelpText = "If true, only the versions in the output folders are used. Required output-path option.")]
        public bool MatchOutputVersion { get; set; }

        [Option('o', "output-path", Default = null, HelpText = "Path of the produced artifacts.")]
        public string? OutputPath { get; set; }

        [Option('s', "sources-path", HelpText = "Source path to search the 'project.assets.json' in.", Required = true)]
        public string SourcesPath { get; set; } = default!;
    }
}