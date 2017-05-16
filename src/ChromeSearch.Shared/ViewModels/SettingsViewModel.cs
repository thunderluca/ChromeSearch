using ChromeSearch.Shared.Helpers;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;

namespace ChromeSearch.Shared.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly INavigationService NavigationService;

        public SettingsViewModel(INavigationService navigationService)
        {
            if (navigationService == null)
            {
                throw new ArgumentNullException(nameof(navigationService));
            }

            this.NavigationService = navigationService;
            this.SaveLastUriEnabled = SettingsHelper.GetSaveLastUriFlag();
        }

        private bool _saveLastUriEnabled;
        private RelayCommand _backCommand;

        public bool SaveLastUriEnabled
        {
            get { return _saveLastUriEnabled; }
            set
            {
                if (!Set(nameof(SaveLastUriEnabled), ref _saveLastUriEnabled, value)) return;

                SettingsHelper.UpdateLastUriSetting(value);
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
                        this.NavigationService.GoBack();
                    });
                }

                return _backCommand;
            }
        }
    }
}
