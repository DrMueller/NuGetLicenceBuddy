using CommandLine;
using JetBrains.Annotations;

namespace Mmu.NuGetLicenceBuddy.Infrastructure.Options.Models
{
    [PublicAPI]
    public class ToolOptions
    {
        [Option('t', "include-transitive", Default = false, HelpText = "Include distinct transitive package licenses.")]
        public bool IncludeTransitiveDependencies { get; set; }

        [Option('s', "sources-path", HelpText = "Source path to search the 'project.assets.json' in.", Required = true)]
        public string SourcesPath { get; set; } = default!;
    }
}