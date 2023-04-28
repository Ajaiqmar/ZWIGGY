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
using Zwiggy.Core.Model;
using Zwiggy.Core.ModelBObj;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Zwiggy.View.User_Control
{
    public sealed partial class RestaurantGridViewControl : UserControl
    {
        public event Action<RestaurantBObj> OnGridItemClick;

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<RestaurantBObj>), typeof(RestaurantGridViewControl), new PropertyMetadata(null));

        public ObservableCollection<RestaurantBObj> ItemsSource
        {
            get { return (ObservableCollection<RestaurantBObj>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public RestaurantGridViewControl()
        {
            this.InitializeComponent();
        }

        private void OnRestaurantSelect(object sender, ItemClickEventArgs e)
        {
            RestaurantBObj selectedRestaurant = e.ClickedItem as RestaurantBObj;
            OnGridItemClick.Invoke(selectedRestaurant);
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
    }
}
