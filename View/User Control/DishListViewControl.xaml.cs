using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class DishListViewControl : UserControl
    {
        public event Action<DishBObj> AddDish;
        public event Action<DishBObj> RemoveDish;

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<DishBObj>), typeof(DishListViewControl), new PropertyMetadata(null));

        public ObservableCollection<DishBObj> ItemsSource
        {
            get { return (ObservableCollection<DishBObj>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public DishListViewControl()
        {
            this.InitializeComponent();
        }

        private void OnRemoveDishClicked(object sender, RoutedEventArgs e)
        {
            DishBObj dish = (sender as Button).DataContext as DishBObj;
            RemoveDish.Invoke(dish);
        }

        private void OnAddDishClicked(object sender, RoutedEventArgs e)
        {
            DishBObj dish = (sender as Button).DataContext as DishBObj;
            AddDish.Invoke(dish);
        }
    }
}
