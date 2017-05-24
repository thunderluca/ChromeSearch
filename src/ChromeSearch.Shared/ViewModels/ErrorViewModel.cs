using ChromeSearch.Shared.Helpers;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace ChromeSearch.Shared.ViewModels
{
    public class ErrorViewModel : ViewModelBase
    {
        private int? _errorCode;
        private bool _useBlueScreen, _visible;
        private SolidColorBrush _backgroundBrush, _foregroundBrush;

        public int? ErrorCode
        {
            get { return _errorCode; }
            set { Set(nameof(ErrorCode), ref _errorCode, value); }
        }
        public bool Visible
        {
            get { return _visible; }
            set { Set(nameof(Visible), ref _visible, value); }
        }

        public bool UseBlueScreen
        {
            get { return _useBlueScreen; }
            set { Set(nameof(UseBlueScreen), ref _useBlueScreen, value); }
        }

        public SolidColorBrush BackgroundBrush
        {
            get { return _backgroundBrush; }
            private set { Set(nameof(BackgroundBrush), ref _backgroundBrush, value); }
        }

        public SolidColorBrush ForegroundBrush
        {
            get { return _foregroundBrush; }
            private set { Set(nameof(ForegroundBrush), ref _foregroundBrush, value); }
        }

        public void UpdateErrorCode(int errorCode)
        {
            this.UseBlueScreen = SettingsHelper.GetBlueScreenFlag();
            this.BackgroundBrush = new SolidColorBrush(UseBlueScreen ? Constants.GoogleBlueColor : Colors.Transparent);
            this.ForegroundBrush = new SolidColorBrush(UseBlueScreen ? Colors.White : Constants.GoogleForegroundColor);
            this.ErrorCode = errorCode;
            this.Visible = true;
        }

        public void ResetErrorCode()
        {
            this.ErrorCode = null;
            this.Visible = false;
        }
    }
}
