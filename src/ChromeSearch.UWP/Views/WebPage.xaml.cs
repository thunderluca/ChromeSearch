using System;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using ChromeSearch.Shared.ViewModels;
using Windows.UI.Xaml.Navigation;
using Windows.Phone.UI.Input;

namespace ChromeSearch.UWP.Views
{
    public sealed partial class WebPage : Page
    {
        private WebViewModel ViewModel
        {
            get { return this.DataContext as WebViewModel; }
        }

        public WebPage()
        {
            this.InitializeComponent();

            this.ViewModel.SetWebViewInstance(this.WebView);

            if (!App.IsMobile) return;

            this.ViewModel.SetStatusBarInstance(StatusBar.GetForCurrentView());
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (App.IsMobile)
                HardwareButtons.BackPressed += OnBackPressed;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (App.IsMobile)
                HardwareButtons.BackPressed -= OnBackPressed;
        }

        private async void OnNewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            args.Handled = true;
            await Launcher.LaunchUriAsync(args.Referrer);
        }

        private void OnBackPressed(object sender, BackPressedEventArgs e)
        {
            if (!this.WebView.CanGoBack) return;

            e.Handled = true;
            this.WebView.GoBack();
        }

        private void OnPermissionRequested(WebView sender, WebViewPermissionRequestedEventArgs args)
        {
            args.PermissionRequest.Allow();
        }
    }
}
