using System.Diagnostics;
using System.Text;
using CommandLine;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions;
using Mmu.NuGetLicenceBuddy.Infrastructure.Options.Models;

namespace Mmu.NuGetLicenceBuddy.Infrastructure.Options.Services.Servants.Implementation
{
    public class OptionsMarkdownTableFactory : IOptionsMarkdownTableFactory
    {
        public void CreateTable()
        {
            var optionsAttrType = typeof(OptionAttribute);

            var optionsAttributes = typeof(ToolOptions)
                .GetProperties()
                .SelectMany(f => f.GetCustomAttributes(optionsAttrType, false))
                .Cast<OptionAttribute>()
                .OrderBy(f => f.ShortName);

            var sb = new StringBuilder();
            sb.AppendLine("| Option | Description |");
            sb.AppendLine("| ------ | ------------------------- |");

            var markDownItems = optionsAttributes.Select(attr => new
            {
                Option = $"`-{attr.ShortName}\\|--{attr.LongName}`",
                Description = attr.HelpText
            }).ToList();

            var markdownTable = markDownItems.ToMarkdownTable();

            Debug.WriteLine(markdownTable);
        }
    }
}