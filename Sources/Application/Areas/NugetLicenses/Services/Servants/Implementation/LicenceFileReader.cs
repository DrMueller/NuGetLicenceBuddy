using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Models;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes.Implementation;
using Mmu.NuGetLicenceBuddy.Infrastructure.Logging.Services;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Services.Servants.Implementation
{
    [UsedImplicitly]
    public class LicenceFileReader(
        ILoggingService logger,
        IHttpClientFactory httpClientFactory)
        : ILicenceFileReader
    {
        public async Task<Maybe<Licence>> TryReadingAsync(string licenceUrl)
        {
            if (string.IsNullOrEmpty(licenceUrl))
            {
                return None.Value;
            }

            if (LicenceUrls.Map.TryGetValue(licenceUrl, out var licence))
            {
                return licence;
            }

            return await
                TryDownloadingLicenceContentAsync(licenceUrl)
                    .BindAsync(Licence.TryFindingByText);
        }

        private static string RestructureGitHubUrl(string urlWithoutExtension)
        {
            return urlWithoutExtension
                .Replace("https://github.com/", "https://raw.githubusercontent.com/")
                .Replace("/blob/", "/");
        }

        private async Task<Maybe<string>> TryDownloadingLicenceContentAsync(string licenceUrl)
        {
            using var client = httpClientFactory.CreateClient();

            licenceUrl = RestructureGitHubUrl(licenceUrl);
            var extension = Path.GetExtension(licenceUrl);

            if (!string.IsNullOrEmpty(extension))
            {
                licenceUrl = licenceUrl.Replace(extension, string.Empty);
            }

            try
            {
                var licenseContent = await client.GetStringAsync(licenceUrl);

                return licenseContent;
            }
            catch (Exception ex)
            {
                logger.LogError($"Could not download from {licenceUrl}. Error: {ex.Message}.");

                return None.Value;
            }
        }
    }
}