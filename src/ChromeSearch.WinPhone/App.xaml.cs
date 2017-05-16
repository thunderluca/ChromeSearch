using ChromeSearch.WinPhone.Views;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace ChromeSearch.WinPhone
{
    public sealed partial class App : Application
    {
        private TransitionCollection transitions;
        
        public App()
        {
            this.InitializeComponent();
            this.RegisterNavigationService();
            this.Suspending += this.OnSuspending;
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
                rootFrame = new Frame
                {
                    CacheSize = 1,
                    Language = Windows.Globalization.ApplicationLanguages.Languages[0]
                };

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }
                
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
                
                if (!rootFrame.Navigate(typeof(WebPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            
            Window.Current.Activate();
        }
        
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
        
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
}