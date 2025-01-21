using System.Reflection;
using System.Text;

namespace Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions
{
    public static class EnumerableExtensions
    {
        public static async Task<IEnumerable<TResult>> SelectAsync<TResult, TSource>(this IEnumerable<TSource> source, Func<TSource, Task<TResult>> selector)
        {
            var result = new List<TResult>();

            foreach (var entry in source)
            {
                var selectorResult = await selector(entry);
                result.Add(selectorResult);
            }

            return result;
        }

        public static string ToMarkdownTable<T>(this IReadOnlyCollection<T> source)
        {
            if (!source.Any())
            {
                return string.Empty;
            }

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if (!properties.Any())
            {
                return string.Empty;
            }

            var columnWidths = properties
                .Select(p => Math.Max(
                    p.Name.Length,
                    source.Max(item => p.GetValue(item)?.ToString()?.Length ?? 0)
                )).ToArray();

            var markdownBuilder = new StringBuilder();

            var headerRow = string.Join(" | ", properties.Select((p, i) => p.Name.PadRight(columnWidths[i])));
            markdownBuilder.AppendLine(headerRow);

            var separatorRow = string.Join(" | ", columnWidths.Select(w => new string('-', w)));
            markdownBuilder.AppendLine(separatorRow);

            foreach (var item in source)
            {
                var row = string.Join(" | ", properties.Select((p, i) =>
                    (p.GetValue(item)?.ToString() ?? string.Empty).PadRight(columnWidths[i])));
                markdownBuilder.AppendLine(row);
            }

            var sb = markdownBuilder.ToString();

            return sb.Trim();
        }
    }
}