using JetBrains.Annotations;
using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes;

namespace Mmu.NuGetLicenceBuddy.Areas.LicenceFetching.Models
{
    [PublicAPI]
    public class Licence(
        string identifier,
        string name,
        string licenceTextPart)
    {
        private static readonly IReadOnlyCollection<Licence> _allLicences = new List<Licence>
        {
            Apache2,
            Glp2,
            Mit,
            MsPl,
            MsEula,
            MsEulaNoRedistributable
        };

        public static Licence Apache2 => new("Apache-2.0", "Apache 2", "apache license");
        public static Licence Glp2 => new("GLP2-2.0", "Glp2 2.0", "gpl");
        public static Licence Mit => new("MIT", "MIT", "mit license");
        public static Licence MsEula => new("MS-EULA", "MS EULA", "todo");
        public static Licence MsEulaNoRedistributable => new("MS-EULA-Non-Redistributable", "MS-EULA Non Redistributable", "todo");
        public static Licence MsPl => new("MS-PL", "MS PL", "todo");
        public static Licence None => new("None", "None", "None");

        public string Identifier { get; } = identifier;

        public string Name { get; } = name;
        private string LicenceTextPart { get; } = licenceTextPart;

        public static Maybe<Licence> TryFindingByText(string licenceText)
        {
            var lowerText = licenceText.ToLower();
            var licence = _allLicences.SingleOrDefault(f => lowerText.Contains(f.LicenceTextPart));

            return MaybeFactory.CreateFromNullable(licence);
        }
    }
}