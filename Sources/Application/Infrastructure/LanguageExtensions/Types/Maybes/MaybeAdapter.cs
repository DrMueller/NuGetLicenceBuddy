using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Eithers;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Eithers.Implementation;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes.Implementation;

namespace Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes
{
    [PublicAPI]
    public static class MaybeAdapter
    {
        public static async Task<Maybe<TNew>> BindAsync<T, TNew>(
            this Task<Maybe<T>> maybeTask,
            Func<T, Maybe<TNew>> map)
        {
            var maybe = await maybeTask;

            if (maybe is None<T>)
            {
                return None.Value;
            }

            var someValue = (Some<T>)maybe;

            return map(someValue);
        }

        public static Maybe<TNew> Map<T, TNew>(this Maybe<T> maybe, Func<T, TNew> map)
        {
            if (maybe is None<T>)
            {
                return None.Value;
            }

            var someValue = (Some<T>)maybe;

            return map(someValue);
        }

        public static async Task<Maybe<TNew>> MapAsync<T, TNew>(
            this Task<Maybe<T>> maybeTask,
            Func<T, TNew> map)
        {
            var maybe = await maybeTask;

            if (maybe is None<T>)
            {
                return None.Value;
            }

            var someValue = (Some<T>)maybe;

            return map(someValue);
        }

        public static async Task<Maybe<TNew>> MapAsync<T, TNew>(this Maybe<T> maybe, Func<T, Task<TNew>> map)
        {
            if (maybe is None<T>)
            {
                return None.Value;
            }

            var someValue = (Some<T>)maybe;

            return await map(someValue);
        }

        public static T Reduce<T>(
            this Maybe<T> maybe,
            Func<T> whenNone)
        {
            if (maybe is None<T>)
            {
                return whenNone();
            }

            return (Some<T>)maybe;
        }

        public static async Task<T> ReduceAsync<T>(
            this Task<Maybe<T>> maybeTask,
            Func<T> whenNone)
        {
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
            var maybe = await maybeTask;
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
            if (maybe is None<T>)
            {
                return whenNone();
            }

            return (Some<T>)maybe;
        }

        public static async Task<T> ReduceAsync<T>(
            this Task<Maybe<T>> maybeTask,
            Func<Task<T>> whenNone)
        {
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
            var maybe = await maybeTask;
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
            if (maybe is None<T>)
            {
                return await whenNone();
            }

            return (Some<T>)maybe;
        }

        public static async Task<T> ReduceAsync<T>(
            this Maybe<T> maybe,
            Func<Task<T>> whenNone)
        {
            if (maybe is None<T>)
            {
                return await whenNone();
            }

            return (Some<T>)maybe;
        }

        public static T? ReduceNullable<T>(
            this Maybe<T> maybe,
            Func<T?> whenNone)
            where T : struct
        {
            if (maybe is None<T>)
            {
                return whenNone();
            }

            return (Some<T>)maybe;
        }

        public static T? ReduceNullable<T>(
            this Maybe<T> maybe,
            Func<T?> whenNone)
        {
            if (maybe is None<T>)
            {
                return whenNone();
            }

            return (Some<T>)maybe;
        }

        public static IEnumerable<T> SelectSome<T>(this IEnumerable<Maybe<T>> maybes)
        {
#pragma warning disable CA2021
            return maybes
                .OfType<Some<T>>()
#pragma warning restore CA2021
                .Select(some => (T)some)
                .ToList();
        }

        [UsedImplicitly]
        public static async Task<IReadOnlyCollection<T>> SelectSomeAsync<T>(this Task<IEnumerable<Maybe<T>>> maybesTask)
        {
#pragma warning disable VSTHRD003
            var maybes = await maybesTask;
#pragma warning restore VSTHRD003
#pragma warning disable CA2021
            return maybes
                .OfType<Some<T>>()
#pragma warning restore CA2021
                .Select(some => (T)some)
                .ToList();
        }

        public static Either<TLeft, T> ToEither<TLeft, T>(this Maybe<T> maybe, Func<TLeft> whenNone)
        {
            if (maybe is None<T>)
            {
                return whenNone();
            }

            var someValue = (Some<T>)maybe;

            return new Right<TLeft, T>(someValue);
        }

        public static T? ToNullable<T>(this Maybe<T> maybe)
        {
            if (maybe is None<T>)
            {
                return default;
            }

            var someValue = (Some<T>)maybe;

            return someValue;
        }

        public static void WhenSome<T>(
            this Maybe<T> maybe,
            Action<T> whenSome)
        {
            if (maybe is None<T>)
            {
                return;
            }

            whenSome((Some<T>)maybe);
        }

        public static async Task WhenSomeAsync<T>(
            this Maybe<T> maybe,
            Func<T, Task> whenSome)
        {
            if (maybe is None<T>)
            {
                return;
            }

            await whenSome((Some<T>)maybe);
        }
    }
}