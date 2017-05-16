using System;
using Windows.Storage;

namespace ChromeSearch.Shared.Helpers
{
    public static class SettingsHelper
    {
        private static ApplicationDataContainer Local
        {
            get { return ApplicationData.Current.LocalSettings; }
        }

        public static void SaveLastUri(Uri uri)
        {
            if (!Local.Values.ContainsKey("lastUri"))
                Local.Values.Add("lastUri", uri.ToString());
            else
                Local.Values["lastUri"] = uri.ToString();
        }

        public static Uri GetLastSavedUri()
        {
            if (!Local.Values.ContainsKey("lastUri"))
                return null;

            var uriString = Local.Values["lastUri"] as string;
            if (string.IsNullOrWhiteSpace(uriString))
                return null;
            try
            {
                return new Uri(uriString);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
