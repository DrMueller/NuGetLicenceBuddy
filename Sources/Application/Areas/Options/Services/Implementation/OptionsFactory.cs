using CommandLine;
using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Areas.Options.Models;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes.Implementation;
using Mmu.NuGetLicenceBuddy.Infrastructure.Logging;

namespace Mmu.NuGetLicenceBuddy.Areas.Options.Services.Implementation
{
    [UsedImplicitly]
    public class OptionsFactory(ILoggingService logger) : IOptionsFactory
    {
        public Maybe<ToolOptions> TryCreating(string[] args)
        {
            var result = Parser.Default.ParseArguments<ToolOptions>(args);

            if (result.Errors.Any())
            {
                var errorMessages = string.Join(Environment.NewLine, result.Errors.Select(f => f.ToString()));
                logger.LogError("Could not parse options: " + errorMessages);
                return None.Value;
            }

            LogOptions(result.Value);
            return result.Value;
        }

        private void LogOptions(ToolOptions options)
        {
            logger.LogDebug("SourcesPath: " + options.SourcesPath);
            logger.LogDebug("IncludeTransitiveDependencies: " + options.IncludeTransitiveDependencies);
        }
    }
}