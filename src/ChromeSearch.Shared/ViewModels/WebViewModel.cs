using ChromeSearch.Shared.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

namespace ChromeSearch.Shared.ViewModels
{
    public class WebViewModel : ViewModelBase
    {
        private bool _customHttpHeaderSet;
        private Uri _capturedUri;
        private WebView _webView;
        private StatusBar _statusBar;

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

        public void SetStatusBarInstance(StatusBar statusBarInstance)
        {
            if (statusBarInstance == null)
            {
                throw new ArgumentNullException(nameof(statusBarInstance));
            }

            _statusBar = statusBarInstance;

            _statusBar.InitializeStatusBarWithGoogleColors();
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
            if (_statusBar == null) return;

            var isSearchUri = GoogleDomainsHelper.IsGoogleSearch(_capturedUri);
            if (!isSearchUri)
                _statusBar.BackgroundColor = Colors.White;
            else
                _statusBar.BackgroundColor = Constants.GoogleStatusBarBackgroundColor;
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