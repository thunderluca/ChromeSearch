using ChromeSearch.Shared.Helpers;
using ChromeSearch.Shared.Messanging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

namespace ChromeSearch.Shared.ViewModels
{
    public class WebViewModel : ViewModelBase
    {
        private bool _customHttpHeaderSet;
        private Uri _capturedUri;
        private WebView _webView;

        private RelayCommand _homeCommand, _backCommand;

        private delegate void NavigateHandler(object sender);
        private event NavigateHandler OnNavigate;

        public void SetWebViewInstance(WebView webViewInstance)
        {
            if (webViewInstance == null)
            {
                throw new ArgumentNullException(nameof(webViewInstance));
            }

            _webView = webViewInstance;

            this.OnNavigate += new NavigateHandler(Navigate);
            _webView.NavigationStarting += OnNavigationStarting;
            _webView.NavigationCompleted += OnNavigationCompleted;

            _webView.Navigate(new Uri(GoogleDomainsHelper.BaseUrl));
        }

        private void OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (_customHttpHeaderSet)
            {
                _customHttpHeaderSet = false;
                return;
            }

            args.Cancel = true;
            this.ManageUri(args.Uri);
        }

        private void CheckUriAndUpdateStatusBar()
        {
            var isSearchUri = GoogleDomainsHelper.IsGoogleSearch(_capturedUri);
            Messenger.Default.Send(new StatusBarMessage { UseHomeColors = !isSearchUri });
        }

        private void OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            if (_capturedUri == null) return;

            this.CheckUriAndUpdateStatusBar();
        }

        public void Navigate(object sender)
        {
            _customHttpHeaderSet = true;

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, _capturedUri);
            httpRequestMessage.Headers.Add(Constants.UserAgentHeader, Constants.AndroidChromeUserAgent);

            this.CheckUriAndUpdateStatusBar();

            _webView.NavigateWithHttpRequestMessage(httpRequestMessage);
        }

        public async void ManageUri(Uri uri)
        {
            _capturedUri = uri;

            var isGoogleUri = GoogleDomainsHelper.IsGoogleUri(_capturedUri);
            var isGoogleService = GoogleDomainsHelper.IsGoogleService(_capturedUri);

            if (!isGoogleUri || isGoogleService)
                await Launcher.LaunchUriAsync(_capturedUri);
            else
                OnNavigate(this);
        }

        public RelayCommand HomeCommand
        {
            get
            {
                if (_homeCommand == null)
                {
                    _homeCommand = new RelayCommand(() =>
                    {
                        _webView.Navigate(new Uri(GoogleDomainsHelper.BaseUrl));
                    });
                }

                return _homeCommand;
            }
        }

        public RelayCommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                {
                    _backCommand = new RelayCommand(() =>
                    {
                        if (_webView.CanGoBack)
                            _webView.GoBack();
                    });
                }

                return _backCommand;
            }
        }
    }
}