namespace modan.FK2.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool ContainsAny<T>(this IEnumerable<T> list, IEnumerable<T>? otherList)
        {
            if (otherList == null)
            {
                return false;
            }

            return list.Intersect(otherList).Any();
        }

        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var entry in list)
            {
                action(entry);
            }
        }

        public static(IEnumerable<T> group1, IEnumerable<T> group2) Outersect<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
        {
            var lst1 = list1.ToList();
            var lst2 = list2.ToList();
            var list1Outersection = lst1.Except(lst2).ToList();
            var list2Outersection = lst2.Except(lst1).ToList();

            return (list1Outersection, list2Outersection);
        }

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

        public static async Task<IReadOnlyCollection<TResult>> SelectAsync<TResult, TSource>(this Task<IReadOnlyCollection<TSource>> sourceTask, Func<TSource, TResult> selector)
        {
#pragma warning disable VSTHRD003
            var source = await sourceTask;
#pragma warning restore VSTHRD003

            return source.Select(selector).ToList();
        }
    }
}