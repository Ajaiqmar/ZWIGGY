using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Zwiggy.DependencyService;
using Zwiggy.View.Templated_Control;
using Zwiggy.ViewModel.Contract;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Zwiggy.Service;
using Windows.Storage;
using Zwiggy.Core.ModelBObj;
using Windows.UI.Xaml.Input;
using Zwiggy.Core.Model;
using System.Diagnostics;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Navigation;
using Zwiggy.View.User_Control;
using Windows.UI.Xaml.Controls.Primitives;
using Zwiggy.View.Contract;
using Windows.UI.Core;
using Microsoft.Extensions.DependencyInjection;
using Zwiggy.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Zwiggy.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfileView : Page,IProfileView
    {
        private ProfileViewModelBase _profileVM;
        private Grid _selectedOrderGrid;

        public ProfileView()
        {
            this.InitializeComponent();

            _profileVM = (ProfileViewModelBase)ActivatorUtilities.CreateInstance<ProfileViewModel>(UIDependencyInjector.ServiceProvider, this);
            _selectedOrderGrid = null;

            switch(ApplicationData.Current.LocalSettings.Values["AccentColorType"] as string)
            {
                case "DarkOrangeGrid":
                    OrangeAccentColorTextblock.Visibility = Visibility.Visible;
                    _profileVM.SelectedAccentColorTextBlock = OrangeAccentColorTextblock;
                    break;
                case "SystemColorGrid":
                    BlueAccentColorTextblock.Visibility = Visibility.Visible;
                    _profileVM.SelectedAccentColorTextBlock = BlueAccentColorTextblock;
                    break;
                case "ChocolateGrid":
                    ChocolateAccentColorTextblock.Visibility = Visibility.Visible;
                    _profileVM.SelectedAccentColorTextBlock = ChocolateAccentColorTextblock;
                    break;
                case "GreenGrid":
                    GreenAccentColorTextblock.Visibility = Visibility.Visible;
                    _profileVM.SelectedAccentColorTextBlock = GreenAccentColorTextblock;
                    break;
                case "TomatoGrid":
                    TomatoAccentColorTextblock.Visibility = Visibility.Visible;
                    _profileVM.SelectedAccentColorTextBlock = TomatoAccentColorTextblock;
                    break;
                case "PaleVioletRedGrid":
                    PVRAccentColorTextblock.Visibility = Visibility.Visible;
                    _profileVM.SelectedAccentColorTextBlock = PVRAccentColorTextblock;
                    break;
                case "GoldenRodGrid":
                    GoldenRodAccentColorTextblock.Visibility = Visibility.Visible;
                    _profileVM.SelectedAccentColorTextBlock = GoldenRodAccentColorTextblock;
                    break;
            }
        }

        public CoreDispatcher GetDispatcher()
        {
            return this.Dispatcher;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _profileVM.GetOrders();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _profileVM.Orders = null;
        }


        private void OnSectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CustomFlipView view = sender as CustomFlipView;

            switch (view.SelectedIndex)
            {
                case 0:
                    _profileVM.GetOrders();
                    break;
                case 1:
                    _selectedOrderGrid = null;
                    _profileVM.GetFavouriteRestaurants();
                    break;
                case 2:
                    _selectedOrderGrid = null;
                    _profileVM.GetAddress();
                    break;
                case 3:
                    _selectedOrderGrid = null;
                    _profileVM.IsFavouritesEmpty = false;
                    _profileVM.IsFavouritesNotEmpty = false;
                    _profileVM.IsOrderEmpty = false;
                    _profileVM.IsOrderNotEmpty = false;
                    _profileVM.IsAddressEmpty = false;
                    _profileVM.IsAddressNotEmpty = false;
                    _profileVM.IsSettingsVisible = true;
                    break;
            }
        }

        private void OnAccentColorSelect(object sender, RoutedEventArgs e)
        {
            _profileVM.SelectedAccentColorTextBlock.Visibility = Visibility.Collapsed;

            Button button = sender as Button;
            Grid grid = button.Content as Grid;
            TextBlock textBlock = grid.Children[1] as TextBlock;

            textBlock.Visibility = Visibility.Visible;

            AppearanceService.ChangeAccentColor(grid.Name);

            _profileVM.SelectedAccentColorTextBlock = textBlock;
        }

        private void OnRestaurantSelect(RestaurantBObj selectedRestaurant)
        {
            Frame.Navigate(typeof(DishView),selectedRestaurant);
        }

        private void OnRemoveAddressSelected(object sender, RoutedEventArgs e)
        {
            Address address = ((sender as Button).DataContext as Address);
            _profileVM.RemoveAddress(address);
        }

        private void OnOrderItemPointerEnter(object sender, PointerRoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            grid.Background = new SolidColorBrush(Color.FromArgb(255, 235, 235, 235));
            grid.BorderBrush = new SolidColorBrush(Colors.Gray);
        }

        private void OnOrderItemPointerExited(object sender, PointerRoutedEventArgs e)
        {
            Grid grid = sender as Grid;

            if(grid != _selectedOrderGrid || Window.Current.Bounds.Width < 930)
            {
                grid.Background = new SolidColorBrush(Colors.White);
                grid.BorderBrush = new SolidColorBrush(Colors.Transparent);
            }
        }

        private void OnListItemsLoaded(object sender, RoutedEventArgs e)
        {
            Grid grid = sender as Grid;

            if((grid.DataContext as OrderBObj) == null)
            {
                return;
            }

            if (_selectedOrderGrid == null)
            {
                _selectedOrderGrid = grid;
                if(Window.Current.Bounds.Width > 930)
                {
                    grid.Background = new SolidColorBrush(Color.FromArgb(255, 235, 235, 235));
                    grid.BorderBrush = new SolidColorBrush(Colors.Gray);
                }
                _profileVM.GetOrderDetails(grid.DataContext as OrderBObj);
            }
            else if ((_selectedOrderGrid.DataContext as OrderBObj).Id.Equals((grid.DataContext as OrderBObj).Id))
            {
                _selectedOrderGrid = grid;
                grid.Background = new SolidColorBrush(Color.FromArgb(255, 235, 235, 235));
                grid.BorderBrush = new SolidColorBrush(Colors.Gray);
            }
            else if(!(_selectedOrderGrid.DataContext as OrderBObj).Id.Equals((grid.DataContext as OrderBObj).Id))
            {
                grid.Background = new SolidColorBrush(Colors.White);
                grid.BorderBrush = new SolidColorBrush(Colors.Transparent);
            }
        }

        private void OnOrderItemPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if(Window.Current.Bounds.Width < 930)
            {
                Grid itemGrid = sender as Grid;

                _selectedOrderGrid = itemGrid;

                _profileVM.GetOrderDetails(itemGrid.DataContext as OrderBObj);

                OrderDetailsPane.Visibility = Visibility.Visible;
                OrderSectionFirstColumn.Width = GridLength.Auto;
                OrderSectionSecondColumn.Width = new GridLength(1,GridUnitType.Star);
                OrderDetailsListView.Visibility = Visibility.Collapsed;
                OrderSectionFirstColumn.MinWidth = 0;
                return;
            }

            _selectedOrderGrid.Background = new SolidColorBrush(Colors.White);
            _selectedOrderGrid.BorderBrush = new SolidColorBrush(Colors.Transparent);

            Grid grid = sender as Grid;

            _selectedOrderGrid = grid;
            grid.Background = new SolidColorBrush(Color.FromArgb(255, 235, 235, 235));
            grid.BorderBrush = new SolidColorBrush(Colors.Gray);

            _profileVM.GetOrderDetails(grid.DataContext as OrderBObj);
        }

        private void OnProfileViewSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(Window.Current.Bounds.Width < 930 && _selectedOrderGrid != null)
            {
                _selectedOrderGrid.Background = new SolidColorBrush(Colors.White);
                _selectedOrderGrid.BorderBrush = new SolidColorBrush(Colors.Transparent);
            }
            else if(_selectedOrderGrid != null)
            {
                OrderSectionFirstColumn.Width = GridLength.Auto;
                OrderSectionSecondColumn.Width = new GridLength(1, GridUnitType.Star);
                OrderDetailsListView.Visibility = Visibility.Visible;
                OrderDetailsPane.Visibility = Visibility.Visible;
                OrderSectionFirstColumn.MinWidth = 390;
                _selectedOrderGrid.Background = new SolidColorBrush(Color.FromArgb(255, 235, 235, 235));
                _selectedOrderGrid.BorderBrush = new SolidColorBrush(Colors.Gray);
            }
        }

        private void OnBackButtonPressed(object sender, EventArgs e)
        {
            OrderDetailsPane.Visibility = Visibility.Collapsed;
            OrderSectionFirstColumn.Width = new GridLength(1, GridUnitType.Star);
            OrderSectionSecondColumn.Width = GridLength.Auto;
            OrderDetailsListView.Visibility = Visibility.Visible;
            OrderSectionFirstColumn.MinWidth = 390;
        }

        private void OnFilterTypeSelected(object sender, RoutedEventArgs e)
        {
            ToggleButton selectedButton = sender as ToggleButton;

            OrdersNavigationButton.IsChecked = false;
            FavouritesNavigationButton.IsChecked = false;
            AddressesNavigationButton.IsChecked = false;
            SettingsNavigationButton.IsChecked = false;

            selectedButton.IsChecked = true;

            if(selectedButton == OrdersNavigationButton)
            {
                _profileVM.GetOrders();
            }
            else if (selectedButton == FavouritesNavigationButton)
            {
                _selectedOrderGrid = null;
                _profileVM.GetFavouriteRestaurants();
            }
            else if(selectedButton == AddressesNavigationButton)
            {
                _selectedOrderGrid = null;
                _profileVM.GetAddress();
            }
            else if(selectedButton == SettingsNavigationButton)
            {
                _selectedOrderGrid = null;
                _profileVM.IsFavouritesEmpty = false;
                _profileVM.IsFavouritesNotEmpty = false;
                _profileVM.IsOrderEmpty = false;
                _profileVM.IsOrderNotEmpty = false;
                _profileVM.IsAddressEmpty = false;
                _profileVM.IsAddressNotEmpty = false;
                _profileVM.IsSettingsVisible = true;
            }
        }
    }
}
