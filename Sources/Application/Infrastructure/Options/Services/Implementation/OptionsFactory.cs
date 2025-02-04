﻿using CommandLine;
using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes.Implementation;
using Mmu.NuGetLicenceBuddy.Infrastructure.Logging.Services;
using Mmu.NuGetLicenceBuddy.Infrastructure.Options.Models;
using Mmu.NuGetLicenceBuddy.Infrastructure.Options.Services.Servants;
using Mmu.NuGetLicenceBuddy.Infrastructure.Outputs.Services;

namespace Mmu.NuGetLicenceBuddy.Infrastructure.Options.Services.Implementation
{
    [UsedImplicitly]
    public class OptionsFactory(
        ILoggingService logger,
        IOptionsMarkdownTableFactory tableFactory,
        ITaskOutputService taskOutputService) : IOptionsFactory
    {
        public Maybe<ToolOptions> TryCreating(string[] args)
        {
            var result = Parser.Default.ParseArguments<ToolOptions>(args);

            if (result.Errors.Any())
            {
                var errorMessages = string.Join(Environment.NewLine, result.Errors.Select(f => f.ToString()));
                logger.LogError("Could not parse options: " + errorMessages);

                taskOutputService.FailTask();

                return None.Value;
            }

            tableFactory.CreateTable();
            LogOptions(result.Value);

            return ValidateOptions(result.Value);
        }

        private void LogOptions(ToolOptions options)
        {
            logger.LogDebug("SourcesPath: " + options.SourcesPath);
            logger.LogDebug("IncludeTransitiveDependencies: " + options.IncludeTransitiveDependencies);
            logger.LogDebug("AllowedLicences: " + options.AllowedLicences);
            logger.LogDebug("ExcludePackagesFilterOption: " + options.ExcludePackagesFilterOption);
            logger.LogDebug("MatchOutputVersion: " + options.MatchOutputVersion);
            logger.LogDebug("OutputPath: " + options.OutputPath);
            logger.LogDebug("SourcesPath: " + options.SourcesPath);
        }

        private Maybe<ToolOptions> ValidateOptions(ToolOptions options)
        {
            if (options.MatchOutputVersion && string.IsNullOrEmpty(options.OutputPath))
            {
                logger.LogError("MatchOutputVersion is set, but no OutputPath is provided.");
                taskOutputService.FailTask();

                return None.Value;
            }

            return options;
        }
    }
}