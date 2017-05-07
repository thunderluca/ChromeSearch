using GalaSoft.MvvmLight;
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
        }

        public WebViewModel Web => ServiceLocator.Current.GetInstance<WebViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}