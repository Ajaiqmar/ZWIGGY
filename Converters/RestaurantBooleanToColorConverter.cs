using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Zwiggy.Converters
{
    class RestaurantBooleanToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isFavourite = (bool)value;

            if (isFavourite)
            {
                return new SolidColorBrush(Color.FromArgb(255, 254, 1, 0));
            }

            return new SolidColorBrush(Colors.Transparent); ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
