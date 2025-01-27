using Mmu.NuGetLicenceBuddy.Areas.LicenceFetching.Models;
using Mmu.NuGetLicenceBuddy.Infrastructure.Logging.Services;
using Mmu.NuGetLicenceBuddy.Infrastructure.Outputs.Services;

namespace Mmu.NuGetLicenceBuddy.Areas.AllowedLicences.Services.Implementation
{
    public class AllowedLicencesChecker(
        ILoggingService logger,
        ITaskOutputService taskOutputService) : IAllowedLicencesChecker
    {
        public void CheckLicences(IReadOnlyCollection<NugetLicence> licences, string allowedLicences)
        {
            if (string.IsNullOrEmpty(allowedLicences))
            {
                logger.LogDebug("No allowed licences defined, skipping check.");
                taskOutputService.SucceedTask();

                return;
            }

            var allowedLicencesList = allowedLicences
                .Replace("'", string.Empty)
                .Split([','], StringSplitOptions.RemoveEmptyEntries)
                .Select(f => f.ToLower().ToUpper())
                .ToList();

            var foundLicenceIds = licences
                .Select(f => f.Licence.Identifier)
                .Select(f => f.ToUpper())
                .Distinct()
                .ToList();

            var failingLicences = foundLicenceIds
                .Where(f => !allowedLicencesList.Contains(f))
                .ToList();

            logger.LogDebug($"Allowed licences: {string.Join(", ", allowedLicencesList)}");
            logger.LogDebug($"Found licences: {string.Join(", ", foundLicenceIds)}");
            logger.LogDebug($"Failing licences: {string.Join(", ", failingLicences)}");

            if (!failingLicences.Any())
            {
                logger.LogDebug("No forbidden licences found.");
                taskOutputService.SucceedTask();

                return;
            }

            var failingLicencesText = string.Join(", ", failingLicences);
            logger.LogError("The following licences are not allowed: " + failingLicencesText);
            taskOutputService.FailTask();
        }
    }
}