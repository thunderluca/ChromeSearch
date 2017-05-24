using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace ChromeSearch.Shared.Converters
{
    public class BooleanToAppBarClosedDisplayMode : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
            {
                return (bool)value ? AppBarClosedDisplayMode.Minimal : AppBarClosedDisplayMode.Compact;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
