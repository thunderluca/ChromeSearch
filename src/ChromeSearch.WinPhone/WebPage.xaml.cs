using ChromeSearch.Shared.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using GalaSoft.MvvmLight.Messaging;
using ChromeSearch.Shared.Messanging;
using Windows.Phone.UI.Input;
using ChromeSearch.Shared;
using Windows.UI;

namespace ChromeSearch.WinPhone
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

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.ViewModel.SetWebViewInstance(this.WebView);

            StatusBar.GetForCurrentView().BackgroundOpacity = 1.0;
            StatusBar.GetForCurrentView().ForegroundColor = Constants.GoogleForegroundColor;
            StatusBar.GetForCurrentView().BackgroundColor = Colors.White;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Messenger.Default.Register<StatusBarMessage>(this, ManageStatusBarMessage);

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

            HardwareButtons.BackPressed -= OnBackPressed;
        }

        private void OnBackPressed(object sender, BackPressedEventArgs e)
        {
            if (!this.WebView.CanGoBack) return;

            e.Handled = true;
            this.WebView.GoBack();
        }
    }
}
