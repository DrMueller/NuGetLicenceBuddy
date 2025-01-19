namespace Mmu.NuGetLicenceBuddy.Areas.LicenceFetching.Models
{
    internal static class LicenceUrls
    {
        public static IDictionary<string, Licence> Map { get; } = new Dictionary<string, Licence>
        {
            { "https://licenses.nuget.org/Apache-2.0", Licence.Apache2 },
            { "http://www.apache.org/licenses/LICENSE-2.0.html", Licence.Apache2 },
            { "http://www.apache.org/licenses/LICENSE-2.0", Licence.Apache2 },
            { "http://opensource.org/licenses/Apache-2.0", Licence.Apache2 },
            { "http://aws.amazon.com/apache2.0/", Licence.Apache2 },
            { "http://logging.apache.org/log4net/license.html", Licence.Apache2 },
            { "https://github.com/owin-contrib/owin-hosting/blob/master/LICENSE.txt", Licence.Apache2 },
            { "https://raw.githubusercontent.com/aspnet/Home/2.0.0/LICENSE.txt", Licence.Apache2 },
            { "https://github.com/Microsoft/Microsoft.IO.RecyclableMemoryStream/blob/master/LICENSE", Licence.Mit },
            { "https://github.com/AutoMapper/AutoMapper/blob/master/LICENSE.txt", Licence.Mit },
            { "https://github.com/zzzprojects/html-agility-pack/blob/master/LICENSE", Licence.Mit },
            { "https://raw.githubusercontent.com/hey-red/markdownsharp/master/LICENSE", Licence.Mit },
            { "https://raw.github.com/JamesNK/Newtonsoft.Json/master/LICENSE.md", Licence.Mit },
            { "https://licenses.nuget.org/Mit", Licence.Mit },
            { "http://opensource.org/licenses/Mit", Licence.Mit },
            { "http://www.opensource.org/licenses/mit-license.php", Licence.Mit },
            { "http://max.mit-license.org/", Licence.Mit },
            { "https://github.com/dotnet/corefx/blob/master/LICENSE.TXT", Licence.Mit },
            { "http://www.gnu.org/licenses/old-licenses/gpl-2.0.html", Licence.Glp2 },
            { "http://opensource.org/licenses/MS-PL", Licence.MsPl },
            { "http://www.opensource.org/licenses/ms-pl", Licence.MsPl },
            { "https://www.microsoft.com/web/webpi/eula/aspnetmvc3update-eula.htm", Licence.MsEula },
            { "http://go.microsoft.com/fwlink/?LinkID=214339", Licence.MsEula },
            { "https://www.microsoft.com/web/webpi/eula/net_library_eula_enu.htm", Licence.MsEula },
            { "http://go.microsoft.com/fwlink/?LinkId=329770", Licence.MsEula },
            { "http://go.microsoft.com/fwlink/?LinkId=529443", Licence.MsEula },
            { "https://www.microsoft.com/web/webpi/eula/dotnet_library_license_non_redistributable.htm", Licence.MsEulaNoRedistributable },
            { "http://go.microsoft.com/fwlink/?LinkId=529444", Licence.MsEulaNoRedistributable }
        };
    }
}