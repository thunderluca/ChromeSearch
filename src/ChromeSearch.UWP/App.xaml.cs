using ChromeSearch.UWP.Views;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ChromeSearch.UWP
{
    sealed partial class App : Application
    {
        public static bool IsMobile
        {
            get { return ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"); }
        }

        public App()
        {
            this.InitializeComponent();
            this.RegisterNavigationService();
            this.Suspending += OnSuspending;
        }

        private void RegisterNavigationService()
        {
            var navigationService = new NavigationService();
            navigationService.Configure(nameof(WebPage), typeof(WebPage));
            navigationService.Configure(nameof(SettingsPage), typeof(SettingsPage));

            SimpleIoc.Default.Register<INavigationService>(() => navigationService);
        }
        
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }
                
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated) return;

            if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(WebPage), e.Arguments);
            }
            Window.Current.Activate();
        }
        
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
        
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
}
