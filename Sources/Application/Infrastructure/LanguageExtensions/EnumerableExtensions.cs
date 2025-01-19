using System.Reflection;

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
            var properties = typeof(T).GetRuntimeProperties();
            var fields = typeof(T)
                .GetRuntimeFields()
                .Where(f => f.IsPublic);

            var gettables = properties
                .Select(p => new { p.Name, GetValue = (Func<object, object>)p.GetValue!, Type = p.PropertyType }).Union(fields.Select(p => new { p.Name, GetValue = (Func<object, object>)p.GetValue!, Type = p.FieldType }))
                .ToList();

            var maxColumnValues = source
                .Select(x => gettables.Select(p => p.GetValue(x!).ToString()?.Length ?? 0))
                .Union([gettables.Select(p => p.Name.Length)])
                .Aggregate(
                    new int[gettables.Count].AsEnumerable(),
                    (accumulate, x) => accumulate.Zip(x, Math.Max))
                .ToArray();

            var columnNames = gettables.Select(p => p.Name);

            var headerLine = "| " + string.Join(" | ", columnNames.Select((n, i) => n.PadRight(maxColumnValues[i]))) + " |";

            var isNumeric = new Func<Type, bool>(type =>
                type == typeof(byte) ||
                type == typeof(sbyte) ||
                type == typeof(ushort) ||
                type == typeof(uint) ||
                type == typeof(ulong) ||
                type == typeof(short) ||
                type == typeof(int) ||
                type == typeof(long) ||
                type == typeof(decimal) ||
                type == typeof(double) ||
                type == typeof(float));

            var rightAlign = new Func<Type, char>(type => isNumeric(type) ? ':' : ' ');

            var headerDataDividerLine =
                "| " +
                string.Join(
                    "| ",
                    gettables.Select((g, i) => new string('-', maxColumnValues[i]) + rightAlign(g.Type))) +
                "|";

            var lines = new[]
            {
                headerLine,
                headerDataDividerLine,
            }.Union(source.Select(s => "| " + string.Join(" | ", gettables.Select((n, i) => (n.GetValue(s!).ToString() ?? "").PadRight(maxColumnValues[i]))) + " |"));

            return lines.Aggregate((p, c) => p + Environment.NewLine + c);
        }
    }
}