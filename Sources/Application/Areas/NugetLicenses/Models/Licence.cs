using Mmu.NuGetLicenceBuddy.Infrastructure.LanguageExtensions.Types.Maybes;

namespace Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Models
{
    public class Licence
    {
        private static readonly IReadOnlyCollection<Licence> _allLicences = new List<Licence>
        {
            Apache2,
            GPL2,
            MIT,
            MSPL,
            MSEULA,
            MSEULANoRedistributable
        };

        public Licence(
            string identifier,
            string name,
            string licenceTextPart)
        {
            Identifier = identifier;
            Name = name;
            LicenceTextPart = licenceTextPart;
        }

        public static Licence Apache2 => new("Apache-2.0", "Apache 2", "apache license");
        public static Licence GPL2 => new("GPL2-2.0", "GPL2 2.0", "gpl");
        public static Licence MIT => new("MIT", "MIT", "mit license");
        public static Licence MSEULA => new("MS-EULA", "MS EULA", "todo");
        public static Licence MSEULANoRedistributable => new("MS-EULA-Non-Redistributable", "MS-EULA Non Redistributable", "todo");
        public static Licence MSPL => new("MS-PL", "MS PL", "todo");
        public static Licence None => new("None", "None", "None");
        public string Identifier { get; }
        public string LicenceTextPart { get; }
        public string Name { get; }

        public static Licence Find(string identifier)
        {
            return _allLicences.Single(f => f.Identifier == identifier);
        }

        public static Maybe<Licence> TryFindingByText(string licenceText)
        {
            var lowerText = licenceText.ToLower();
            var licence = _allLicences.SingleOrDefault(f => lowerText.Contains(f.LicenceTextPart));
            return MaybeFactory.CreateFromNullable(licence);
        }
    }
}