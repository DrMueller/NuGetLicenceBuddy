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
    }
}