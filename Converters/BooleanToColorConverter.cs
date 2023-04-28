using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Zwiggy.Converters
{
    class BooleanToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isVeg = (bool)value;

            if (isVeg)
            {
                return new SolidColorBrush(Color.FromArgb(255, 15, 138, 101));
            }

            return new SolidColorBrush(Color.FromArgb(255, 228, 59, 79)); ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
