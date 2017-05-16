using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace ChromeSearch.Shared.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<WebViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
        }

        public WebViewModel Web => ServiceLocator.Current.GetInstance<WebViewModel>();

        public SettingsViewModel Settings => ServiceLocator.Current.GetInstance<SettingsViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}