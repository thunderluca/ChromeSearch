using ChromeSearch.Shared.Helpers;
using ChromeSearch.Shared.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using Windows.Foundation;
using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

namespace ChromeSearch.Shared.ViewModels
{
    public class WebViewModel : ViewModelBase
    {
        private readonly INavigationService NavigationService;

        public WebViewModel(INavigationService navigationService)
        {
            if (navigationService == null)
            {
                throw new ArgumentNullException(nameof(navigationService));
            }

            this.NavigationService = navigationService;
        }

        private bool _customHttpHeaderSet, _homeButtonEnabled, _backButtonEnabled, _loadingState;
        private Uri _capturedUri;
        private WebView _webView;
        private StatusBar _statusBar;

        private RelayCommand _homeCommand, _refreshCommand, _backCommand, _settingsCommand;

        private delegate void NavigateHandler(object sender);
        private event NavigateHandler OnNavigate;

        public bool HomeButtonEnabled
        {
            get { return _homeButtonEnabled; }
            set { Set(nameof(HomeButtonEnabled), ref _homeButtonEnabled, value); }
        }

        public bool BackButtonEnabled
        {
            get { return _backButtonEnabled; }
            set { Set(nameof(BackButtonEnabled), ref _backButtonEnabled, value); }
        }

        public bool LoadingState
        {
            get { return _loadingState; }
            set { Set(nameof(LoadingState), ref _loadingState, value); }
        }

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
            //_webView.NavigationFailed += OnNavigationFailed;

            var loadLastSavedUri = SettingsHelper.GetSaveLastUriFlag();
            if (loadLastSavedUri)
            {
                var lastSavedUri = SettingsHelper.GetLastSavedUri();
                if (lastSavedUri != null)
                {
                    _webView.Navigate(lastSavedUri);
                    return;
                }
            }

            _webView.Navigate(new Uri(GoogleDomainsHelper.BaseUrl));
        }

        //private void OnNavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        //{
        //    var messageContentFormat = ResourcesHelper.ResourceLoader.GetString("NavigationFailedMessageContentFormat");
        //    var messageTitle = ResourcesHelper.ResourceLoader.GetString("NavigationFailedMessageTitle");

        //    await new MessageDialog(
        //        content: string.Format(messageContentFormat, (int)e.WebErrorStatus),
        //        title: messageTitle).ShowAsync();
        //}

        public void SetStatusBarInstance(StatusBar statusBarInstance)
        {
            if (statusBarInstance == null)
            {
                throw new ArgumentNullException(nameof(statusBarInstance));
            }

            _statusBar = statusBarInstance;
            _statusBar.InitializeStatusBarWithGoogleColors();

            var showStatusBar = SettingsHelper.GetShowStatusBarFlag();
            this.UpdateStatusBarVisibility(showStatusBar);

            MessengerInstance.Register<StatusBarMessage>(this, message => UpdateStatusBarVisibility(message.ShowStatusBar));
        }

        public async void UpdateStatusBarVisibility(bool showStatusBar)
        {
            if (showStatusBar)
                await _statusBar.ShowAsync();
            else
                await _statusBar.HideAsync();
        }

        private void OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            this.LoadingState = true;

            var isSignOutWorkflow = GoogleDomainsHelper.IsSignOutWorkflow(args.Uri);

            if (_customHttpHeaderSet && !isSignOutWorkflow)
            {
                _customHttpHeaderSet = false;
                return;
            }

            args.Cancel = true;
            SettingsHelper.SaveLastUri(args.Uri);
            if (isSignOutWorkflow)
            {
                var queryParams = new WwwFormUrlDecoder(args.Uri.Query);
                var uri = new Uri(queryParams.GetFirstValueByName("continue"));
                UriHelper.ClearCookiesFor(uri);
                this.ManageUri(uri);
            }
            else
                this.ManageUri(args.Uri);
        }

        private void CheckUriAndUpdateStatusBar()
        {
            var isSearchUri = GoogleDomainsHelper.IsGoogleSearch(_capturedUri);
            if (_statusBar != null)
            {
                _statusBar.BackgroundColor = isSearchUri
                    ? Constants.GoogleStatusBarBackgroundColor
                    : Colors.White;
            }

            this.HomeButtonEnabled = isSearchUri;
            this.BackButtonEnabled = _webView.CanGoBack;
        }

        private void OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            this.LoadingState = false;

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
            {
                this.LoadingState = false;
                await Launcher.LaunchUriAsync(_capturedUri);
            }
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

        public RelayCommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new RelayCommand(() =>
                    {
                        _webView.Refresh();
                    });
                }

                return _refreshCommand;
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

        public RelayCommand SettingsCommand
        {
            get
            {
                if (_settingsCommand == null)
                {
                    _settingsCommand = new RelayCommand(() =>
                    {
                        this.NavigationService.NavigateTo("SettingsPage");
                    });
                }

                return _settingsCommand;
            }
        }
    }
}