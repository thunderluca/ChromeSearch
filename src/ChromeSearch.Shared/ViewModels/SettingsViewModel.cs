using ChromeSearch.Shared.Helpers;
using ChromeSearch.Shared.Messages;
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
            this.ShowStatusBar = SettingsHelper.GetShowStatusBarFlag();
            this.UseBlueScreen = SettingsHelper.GetBlueScreenFlag();
        }

        private bool _saveLastUriEnabled, _showStatusBar, _useBlueScreen;
        private RelayCommand _backCommand, _saveCommand;

        public bool SaveLastUriEnabled
        {
            get { return _saveLastUriEnabled; }
            set
            {
                if (!Set(nameof(SaveLastUriEnabled), ref _saveLastUriEnabled, value)) return;

                SettingsHelper.UpdateLastUriSetting(value);
            }
        }

        public bool ShowStatusBar
        {
            get { return _showStatusBar; }
            set
            {
                if (!Set(nameof(ShowStatusBar), ref _showStatusBar, value)) return;

                SettingsHelper.UpdateStatusBarSetting(value);

                MessengerInstance.Send(new StatusBarMessage(value));
            }
        }

        public bool UseBlueScreen
        {
            get { return _useBlueScreen; }
            set
            {
                if (!Set(nameof(UseBlueScreen), ref _useBlueScreen, value)) return;

                SettingsHelper.UpdateBlueScreenSetting(value);
            }
        }

        public RelayCommand BackCommand
        {
            get
            {
                if (_backCommand == null)
                {
                    _backCommand = new RelayCommand(GoBack);
                }

                return _backCommand;
            }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(GoBack);
                }

                return _saveCommand;
            }
        }

        private void GoBack()
        {
            SettingsHelper.UpdateLastUriSetting(this.SaveLastUriEnabled);
            SettingsHelper.UpdateStatusBarSetting(this.ShowStatusBar);
            SettingsHelper.UpdateBlueScreenSetting(this.UseBlueScreen);

            this.NavigationService.GoBack();
        }
    }
}
