using Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Models;
using Mmu.NuGetLicenceBuddy.Areas.Outputs.Services;
using Mmu.NuGetLicenceBuddy.Infrastructure.Logging.Services;

namespace Mmu.NuGetLicenceBuddy.Areas.AllowedLicences.Services.Implementation
{
    public class AllowedLicencesChecker(
        ILoggingService logger,
        ITaskFailer taskFailer) : IAllowedLicencesChecker
    {
        public void CheckLicences(IReadOnlyCollection<NugetLicence> licences, string allowedLicences)
        {
            if (string.IsNullOrEmpty(allowedLicences))
            {
                logger.LogDebug("No allowed licences defined, skipping check.");

                return;
            }

            var allowedLicencesList = allowedLicences
                .Split([','], StringSplitOptions.RemoveEmptyEntries)
                .Select(f => f.ToLower().ToUpper())
                .ToList();

            var foundLicenceIds =
                licences
                    .Select(f => f.Licence.Identifier)
                    .Select(f => f.ToUpper())
                    .Distinct();

            var failingLicences = foundLicenceIds
                .Where(f => !allowedLicencesList.Contains(f))
                .ToList();

            if (!failingLicences.Any())
            {
                logger.LogDebug("No forbidden licences found.");

                return;
            }

            var failingLicencesText = string.Join(", ", failingLicences);
            logger.LogError("The following licences are not allowed: " + failingLicencesText);
            taskFailer.FailTask();
        }
    }
}