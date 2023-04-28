using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Zwiggy.Core.ModelBObj;
using Zwiggy.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Zwiggy.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RestaurantView : Page
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public RestaurantView()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await RestaurantVM.GetRestaurantsAsync();
            await RestaurantVM.GetTotalRestaurantCountAsync();

            dispatcherTimer.Tick += OnCommandBarResize;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer.Start();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            dispatcherTimer.Stop();
        }

        private void OnRestaurantSelect(RestaurantBObj selectedRestaurant)
        {
            Frame.Navigate(typeof(DishView),selectedRestaurant);
        }

        //private void OnRestaurantContentHover(object sender, PointerRoutedEventArgs e)
        //{
        //    Grid grid = sender as Grid;
        //    grid.Background = new SolidColorBrush(Color.FromArgb(255, 235, 235, 235));
        //    grid.BorderBrush = new SolidColorBrush(Colors.Gray);
        //}

        //private void OnRestaurantContentLeave(object sender, PointerRoutedEventArgs e)
        //{
        //    Grid grid = sender as Grid;
        //    grid.Background = new SolidColorBrush(Colors.White);
        //    grid.BorderBrush = new SolidColorBrush(Colors.Transparent);
        //}

        private void OnCommandBarResize(object sender, object e)
        {
            if (MiddleColumnGrid.ActualWidth > 350 && MyCommandBar.SecondaryCommands.Count > 0)
            {
                ICommandBarElement barElement = MyCommandBar.SecondaryCommands[0];
                MyCommandBar.SecondaryCommands.Remove(barElement);
                ((barElement as AppBarElementContainer).Content as ToggleButton).Margin = new Thickness(0);
                MyCommandBar.PrimaryCommands.Add(barElement);
            }
        }

        private void OnFilterTypeSelected(object sender, RoutedEventArgs e)
        {
            MyCommandBar.IsOpen = false;
            ToggleButton selectedButton = sender as ToggleButton;
            TextBlock selectedTextBlock = (selectedButton.Content as Grid).Children[1] as TextBlock;

            foreach (AppBarElementContainer element in MyCommandBar.PrimaryCommands)
            {
                TextBlock textBlock = ((element.Content as ToggleButton).Content as Grid).Children[1] as TextBlock;

                if(!textBlock.Text.Equals(selectedTextBlock.Text))
                {
                    (element.Content as ToggleButton).IsChecked = false;
                }
            }

            foreach (AppBarElementContainer element in MyCommandBar.SecondaryCommands)
            {
                TextBlock textBlock = ((element.Content as ToggleButton).Content as Grid).Children[1] as TextBlock;

                if (!textBlock.Text.Equals(selectedTextBlock.Text))
                {
                    (element.Content as ToggleButton).IsChecked = false;
                }
            }

            selectedButton.IsChecked = true;

            RestaurantVM.GetRestaurant(selectedTextBlock.Text);
        }

        private void OnRestaurantPageResized(object sender, SizeChangedEventArgs e)
        {
            if (MiddleColumnGrid.ActualWidth == MiddleColumn.MinWidth && MyCommandBar.PrimaryCommands.Count > 0)
            {
                ICommandBarElement barElement = MyCommandBar.PrimaryCommands[MyCommandBar.PrimaryCommands.Count - 1];
                MyCommandBar.PrimaryCommands.Remove(barElement);
                ((barElement as AppBarElementContainer).Content as ToggleButton).Margin = new Thickness(5,5,5,5);
                MyCommandBar.SecondaryCommands.Insert(0, barElement);
            }
        }
    }
}
