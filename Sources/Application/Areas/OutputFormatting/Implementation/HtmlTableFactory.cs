using System.Text;
using Mmu.NuGetLicenceBuddy.Areas.LicenceFetching.Models;

namespace Mmu.NuGetLicenceBuddy.Areas.OutputFormatting.Implementation
{
    public class HtmlTableFactory : IHtmlTableFactory
    {
        private static readonly IReadOnlyCollection<string> _headers = new List<string>
        {
            "NuGet Name",
            "NuGet Version",
            "Licence Name",
            "License URL"
        };

        public string CreateTable(IReadOnlyCollection<NugetLicence> licences)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<table border='1' style='border-collapse:collapse; width:100%;'>");
            sb.AppendLine("<thead><tr>");

            foreach (var header in _headers)
            {
                sb.AppendLine($"<th style='padding:8px; text-align:left;'>{header}</th>");
            }

            sb.AppendLine("</tr></thead>");
            sb.AppendLine("<tbody>");

            foreach (var lic in licences)
            {
                sb.AppendLine("<tr>");

                sb.AppendLine($"<td style='padding:8px;'>{lic.NugetIdentifier}</td>");
                sb.AppendLine($"<td style='padding:8px;'>{lic.NugetVersion}</td>");
                sb.AppendLine($"<td style='padding:8px;'>{lic.Licence.Name}</td>");
                sb.AppendLine($"<td style='padding:8px;'>{lic.NugetLicenceUrl}</td>");

                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");

            return sb.ToString();
        }
    }
}