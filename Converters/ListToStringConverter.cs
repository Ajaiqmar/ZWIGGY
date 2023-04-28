using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Data;
using Zwiggy.Core.Model;

namespace Zwiggy.Converters
{
    class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            IList<Cuisine> cuisines = value as IList<Cuisine>;

            List<string> cuisineList = new List<String>();

            foreach (Cuisine cuisine in cuisines)
            {
                cuisineList.Add(cuisine.Category);
            }

            return string.Join(", ", cuisineList);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
