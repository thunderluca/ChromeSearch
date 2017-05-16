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

        private static void InsertOrUpdate<T>(string key, T value)
        {
            if (!Local.Values.ContainsKey(key))
                Local.Values.Add(key, value.ToString());
            else
                Local.Values[key] = value.ToString();
        }

        public static void SaveLastUri(Uri uri) => InsertOrUpdate("lastUri", uri.ToString());

        public static void UpdateLastUriSetting(bool isEnabled) => InsertOrUpdate("saveUri", isEnabled);

        public static Uri GetLastSavedUri()
        {
            var uriString = GetValue("lastUri");
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

        public static bool GetSaveLastUriFlag()
        {
            var boolString = GetValue("saveUri");
            if (string.IsNullOrWhiteSpace(boolString))
                return true;

            return bool.Parse(boolString);
        }

        private static string GetValue(string key)
        {
            if (!Local.Values.ContainsKey(key))
                return null;

            return Local.Values[key] as string;
        }
    }
}
