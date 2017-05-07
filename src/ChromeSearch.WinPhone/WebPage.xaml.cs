using ChromeSearch.Common.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using GalaSoft.MvvmLight.Messaging;
using ChromeSearch.Common.Messanging;
using Windows.Phone.UI.Input;

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

            HardwareButtons.BackPressed += OnBackPressed;
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
