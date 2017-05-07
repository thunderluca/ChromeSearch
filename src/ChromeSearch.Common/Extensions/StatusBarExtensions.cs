using ChromeSearch.Common;

namespace Windows.UI.ViewManagement
{
    public static class StatusBarExtensions
    {
        public static void InitializeStatusBarWithGoogleColors(this StatusBar statusBar)
        {
            statusBar.BackgroundOpacity = 1.0;
            statusBar.ForegroundColor = Constants.GoogleForegroundColor;
            statusBar.SetGoogleHomeColor();
        }

        public static void SetGoogleHomeColor(this StatusBar statusBar)
        {
            statusBar.BackgroundColor = Colors.White;
        }

        public static void SetGoogleSearchColor(this StatusBar statusBar)
        {
            statusBar.BackgroundColor = Constants.GoogleStatusBarBackgroundColor;
        }
    }
}
