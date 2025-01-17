namespace Mmu.NuGetLicenceBuddy.Areas.NugetLicenses.Models
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
            { "https://github.com/Microsoft/Microsoft.IO.RecyclableMemoryStream/blob/master/LICENSE", Licence.MIT },
            { "https://github.com/AutoMapper/AutoMapper/blob/master/LICENSE.txt", Licence.MIT },
            { "https://github.com/zzzprojects/html-agility-pack/blob/master/LICENSE", Licence.MIT },
            { "https://raw.githubusercontent.com/hey-red/markdownsharp/master/LICENSE", Licence.MIT },
            { "https://raw.github.com/JamesNK/Newtonsoft.Json/master/LICENSE.md", Licence.MIT },
            { "https://licenses.nuget.org/MIT", Licence.MIT },
            { "http://opensource.org/licenses/MIT", Licence.MIT },
            { "http://www.opensource.org/licenses/mit-license.php", Licence.MIT },
            { "http://max.mit-license.org/", Licence.MIT },
            { "https://github.com/dotnet/corefx/blob/master/LICENSE.TXT", Licence.MIT },
            { "http://www.gnu.org/licenses/old-licenses/gpl-2.0.html", Licence.GPL2 },
            { "http://opensource.org/licenses/MS-PL", Licence.MSPL },
            { "http://www.opensource.org/licenses/ms-pl", Licence.MSPL },
            { "https://www.microsoft.com/web/webpi/eula/aspnetmvc3update-eula.htm", Licence.MSEULA },
            { "http://go.microsoft.com/fwlink/?LinkID=214339", Licence.MSEULA },
            { "https://www.microsoft.com/web/webpi/eula/net_library_eula_enu.htm", Licence.MSEULA },
            { "http://go.microsoft.com/fwlink/?LinkId=329770", Licence.MSEULA },
            { "http://go.microsoft.com/fwlink/?LinkId=529443", Licence.MSEULA },
            { "https://www.microsoft.com/web/webpi/eula/dotnet_library_license_non_redistributable.htm", Licence.MSEULANoRedistributable },
            { "http://go.microsoft.com/fwlink/?LinkId=529444", Licence.MSEULANoRedistributable }
        };
    }
}