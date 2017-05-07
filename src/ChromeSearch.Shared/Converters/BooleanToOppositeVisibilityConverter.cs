using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ChromeSearch.Shared.Converters
{
    public class BooleanToOppositeVisibilityConverter : DependencyObject, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is bool)) return value;

            return (bool)value ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
