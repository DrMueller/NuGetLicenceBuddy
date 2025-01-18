using Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Models;
using Mmu.NuGetLicenceBuddy.Areas.Outputs.Services;
using Mmu.NuGetLicenceBuddy.Infrastructure.Logging.Services;

namespace Mmu.NuGetLicenceBuddy.Areas.AllowedLicences.Services.Implementation
{
    public class AllowedLicencesChecker : IAllowedLicencesChecker
    {
        private readonly ILoggingService _logger;
        private readonly ITaskFailer _taskFailer;

        public AllowedLicencesChecker(
            ILoggingService logger,
            ITaskFailer taskFailer)
        {
            _logger = logger;
            _taskFailer = taskFailer;
        }

        public void CheckLicences(IReadOnlyCollection<NugetLicence> licences, string allowedLicences)
        {
            if (string.IsNullOrEmpty(allowedLicences))
            {
                _logger.LogDebug("No allowed licences defined, skipping check.");

                return;
            }

            var allowedLicencesList = allowedLicences
                .Split([','], StringSplitOptions.RemoveEmptyEntries)
                .Select(f => f.ToLower().Trim())
                .ToList();

            var foundLicenceIds =
                licences
                    .Select(f => f.Licence.Identifier)
                    .Select(f => f.ToLower())
                    .Distinct();

            var failingLicences = foundLicenceIds
                .Where(f => !allowedLicencesList.Contains(f))
                .ToList();

            if (!failingLicences.Any())
            {
                _logger.LogDebug("All licences are allowed.");

                return;
            }

            var failingLicencesText = string.Join(", ", failingLicences);
            _logger.LogError("The following licences are not allowed: " + failingLicencesText);
            _taskFailer.FailTask();
        }
    }
}