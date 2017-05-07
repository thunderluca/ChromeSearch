using System;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using ChromeSearch.Shared.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using ChromeSearch.Shared.Messanging;
using Windows.UI.Xaml.Navigation;
using Windows.Phone.UI.Input;
using Windows.UI;
using ChromeSearch.Shared;

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

            StatusBar.GetForCurrentView().BackgroundOpacity = 1.0;
            StatusBar.GetForCurrentView().ForegroundColor = Constants.GoogleForegroundColor;
            StatusBar.GetForCurrentView().BackgroundColor = Colors.White;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Messenger.Default.Register<StatusBarMessage>(this, ManageStatusBarMessage);

            if (App.IsMobile)
                HardwareButtons.BackPressed += OnBackPressed;
        }

        private void ManageStatusBarMessage(StatusBarMessage message)
        {
            if (message.UseHomeColors)
                StatusBar.GetForCurrentView().BackgroundColor = Colors.White;
            else
                StatusBar.GetForCurrentView().BackgroundColor = Constants.GoogleStatusBarBackgroundColor;
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
