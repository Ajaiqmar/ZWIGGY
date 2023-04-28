using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Zwiggy.Core.ModelBObj;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Zwiggy.View.User_Control
{
    public sealed partial class SearchResultRestaurantGridViewControl : UserControl
    {
        public event Action<RestaurantBObj> RestaurantSelected;

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<RestaurantBObj>), typeof(SearchResultRestaurantGridViewControl), new PropertyMetadata(null));

        public ObservableCollection<RestaurantBObj> ItemsSource
        {
            get { return (ObservableCollection<RestaurantBObj>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public SearchResultRestaurantGridViewControl()
        {
            this.InitializeComponent();
        }


        private void OnRestaurantContentHover(object sender, PointerRoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            grid.Background = new SolidColorBrush(Color.FromArgb(255, 235, 235, 235));
            grid.BorderBrush = new SolidColorBrush(Colors.Gray);
        }

        private void OnRestaurantContentLeave(object sender, PointerRoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            grid.Background = new SolidColorBrush(Colors.White);
            grid.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }

        private void OnRestaurantSelect(object sender, ItemClickEventArgs e)
        {
            RestaurantSelected.Invoke(e.ClickedItem as RestaurantBObj);
        }
    }
}
