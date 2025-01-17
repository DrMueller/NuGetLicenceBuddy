namespace Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Eithers.Implementation
{
    public class Left<TLeft, TRight>(TLeft content) : Either<TLeft, TRight>
    {
        private TLeft Content { get; } = content;

        public static implicit operator TLeft(Left<TLeft, TRight> left)
        {
            return left.Content;
        }
    }
}