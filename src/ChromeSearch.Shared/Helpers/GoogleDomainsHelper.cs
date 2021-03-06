﻿using System;
using System.Linq;
using Windows.Foundation;

namespace ChromeSearch.Shared.Helpers
{
    public static class GoogleDomainsHelper
    {
        public static string BaseUrl = "https://www.google.com";

        public static bool IsGoogleUri(Uri uri) => Hosts.Any(host => uri.Host.ToLower().Contains(host));

        public static bool IsGoogleService(Uri uri) => Services.Any(service => uri.Host.ToLower().Contains(service));

        public static bool IsGoogleSearch(Uri uri) => uri.LocalPath.StartsWith(SearchLocalPath);

        public static bool IsSignOutWorkflow(Uri uri)
        {
            var isSignOutNavigation = uri.LocalPath.StartsWith(SignOutOptionsPath);
            if (!isSignOutNavigation) return false;

            var queryParams = new WwwFormUrlDecoder(uri.Query);
            if (queryParams == null || queryParams.Count == 0)
                return false;

            return queryParams.All(kvp => kvp.Name != "hl");
        }

        private const string SignOutOptionsPath = "/SignOutOptions";

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
