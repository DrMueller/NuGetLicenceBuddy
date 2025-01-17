using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes.Implementation;

namespace Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes
{
    public static class MaybeFactory
    {
        public static Maybe<T> CreateFromNullable<T>(T? possiblyNull)
        {
            return possiblyNull == null ? None.Value : possiblyNull;
        }

        public static Maybe<T> CreateFromNullable<T>(T? possiblyNull)
            where T : struct
        {
            return possiblyNull == null ? None.Value : possiblyNull;
        }
    }
}