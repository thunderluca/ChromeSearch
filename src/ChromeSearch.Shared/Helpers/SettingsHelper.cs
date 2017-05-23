using System;
using Windows.Storage;

namespace ChromeSearch.Shared.Helpers
{
    public static class SettingsHelper
    {
        private const string lastUriKey = "lastUri";
        private const string saveUriKey = "saveUri";
        private const string showStatusBarKey = "showStatusBar";
        private const string blueScreenKey = "useBlueScreen";

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

        public static void SaveLastUri(Uri uri) => InsertOrUpdate(lastUriKey, uri.ToString());

        public static void UpdateLastUriSetting(bool isEnabled) => InsertOrUpdate(saveUriKey, isEnabled);

        public static void UpdateStatusBarSetting(bool showStatusBar) => InsertOrUpdate(showStatusBarKey, showStatusBar);

        public static void UpdateBlueScreenSetting(bool useBlueScreen) => InsertOrUpdate(blueScreenKey, useBlueScreen);

        public static Uri GetLastSavedUri()
        {
            var uriString = GetValue(lastUriKey);
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

        public static bool GetSaveLastUriFlag() => GetFlagValue(saveUriKey);

        public static bool GetShowStatusBarFlag() => GetFlagValue(showStatusBarKey);

        public static bool GetBlueScreenFlag() => GetFlagValue(blueScreenKey, defaultValue: false);

        private static bool GetFlagValue(string key, bool defaultValue = true)
        {
            var boolString = GetValue(key);
            if (string.IsNullOrWhiteSpace(boolString))
                return defaultValue;

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
