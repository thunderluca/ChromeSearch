using ChromeSearch.Shared;
using System;
using Windows.Web.Http;
using Windows.System;
using Windows.UI.Xaml.Controls;
using System.Linq;

namespace ChromeSearch.UWP
{
    public sealed partial class WebPage : Page
    {
        private bool _customHttpHeaderSet;
        private Uri _currentUri;

        private delegate void NavigateHandler(object sender);
        private event NavigateHandler OnNavigate;

        public WebPage()
        {
            this.InitializeComponent();

            this.OnNavigate += new NavigateHandler(Navigate);
            _customHttpHeaderSet = false;
            WebView.Navigate(new Uri(GoogleDomainsHelper.BaseUrl));
        }

        private void Navigate(object sender)
        {
            _customHttpHeaderSet = true;

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, _currentUri);
            httpRequestMessage.Headers.Add("User-Agent", Constants.AndroidChromeUserAgent);

            WebView.NavigateWithHttpRequestMessage(httpRequestMessage);
        }

        private async void OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (_customHttpHeaderSet)
            {
                _customHttpHeaderSet = false;
                return;
            }
            
            _currentUri = args.Uri;

            var googleUri = GoogleDomainsHelper.Hosts.Any(host => _currentUri.Host.ToLower().Contains(host));
            if (!googleUri)
                await Launcher.LaunchUriAsync(_currentUri);
            else
                OnNavigate(this);
        }

        private async void OnNewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            args.Handled = true;
            await Launcher.LaunchUriAsync(args.Referrer);
        }
    }
}
