using System;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using ChromeSearch.Common.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using ChromeSearch.Common.Messanging;
using Windows.UI.Xaml.Navigation;
using Windows.Phone.UI.Input;

namespace ChromeSearch.UWP
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

            StatusBar.GetForCurrentView().InitializeStatusBarWithGoogleColors();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Messenger.Default.Register<StatusBarMessage>(this, msg =>
            {
                if (msg.UseHomeColors)
                    StatusBar.GetForCurrentView().SetGoogleHomeColor();
                else
                    StatusBar.GetForCurrentView().SetGoogleSearchColor();
            });

            if (App.IsMobile)
                HardwareButtons.BackPressed += OnBackPressed;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Messenger.Default.Unregister<StatusBarMessage>(this);

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
    }
}
