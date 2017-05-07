using System;
using System.Linq;

namespace ChromeSearch.Shared.Helpers
{
    public static class GoogleDomainsHelper
    {
        public static string BaseUrl = "https://www.google.com";

        public static bool IsGoogleUri(Uri uri) => Hosts.Any(host => uri.Host.ToLower().Contains(host));

        public static bool IsGoogleService(Uri uri) => Services.Any(service => uri.Host.ToLower().Contains(service));

        public static bool IsGoogleSearch(Uri uri) => uri.LocalPath.StartsWith(SearchLocalPath);

        private const string SearchLocalPath = "/search";

        public static string[] Hosts = new[] 
        {
            "google",
            "account.youtube",
            "accounts.youtube"
        };

        public static string[] Services = new[]
        {
            "maps",
            "play",
            "news",
            "mail",
            "drive",
            "hangout",
            "plus",
            "translate",
            "photos"
        };
    }
}
