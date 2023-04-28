using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Notifications;
using Zwiggy.DependencyService;
using Zwiggy.View.Contract;
using Zwiggy.View.User_Control;
using Zwiggy.ViewModel;
using Zwiggy.ViewModel.Contract;
using Zwiggy.ViewObj;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Zwiggy.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DishView : Page,IDishView
    {
        private DishViewModelBase _dishVM;
        private DispatcherTimer _dispatcherTimer = new DispatcherTimer();

        public DishView()
        {
            this.InitializeComponent();
            _dishVM = (DishViewModelBase)ActivatorUtilities.CreateInstance<DishViewModel>(UIDependencyInjector.ServiceProvider, this);
        }

        public CoreDispatcher GetDispatcher()
        {
            return this.Dispatcher;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame.Navigate(typeof(RestaurantView));
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            RestaurantBObj restaurant = e.Parameter as RestaurantBObj;

            _dishVM.RestaurantInView = restaurant;
            await _dishVM.IsFavouriteRestaurantAsync();
            _dishVM.GetCart();

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            Button button = VegButton;
            Grid grid = button.Content as Grid;
            Grid textBlockContainer = grid.Children[0] as Grid;
            Border border = textBlockContainer.Children[1] as Border;
            TextBlock textBlock = border.Child as TextBlock;

            if (ApplicationData.Current.LocalSettings.Values["VegFilter"].Equals("1"))
            {
                textBlockContainer.HorizontalAlignment = HorizontalAlignment.Right;
                grid.Background = new SolidColorBrush(Color.FromArgb(255, 0, 128, 0));
                textBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 128, 0));

                await _dishVM.GetDishesAsync(true);

                DishCategoryListView.SelectedIndex = 0;
            }
            else
            {
                textBlockContainer.HorizontalAlignment = HorizontalAlignment.Left;
                grid.Background = new SolidColorBrush(Color.FromArgb(255, 172, 172, 172));
                textBlock.Foreground = new SolidColorBrush(Colors.Transparent);

                await _dishVM.GetDishesAsync(false);

                DishCategoryListView.SelectedIndex = 0;
            }

            ZwiggyNotification.CartUpdated += _dishVM.UpdateDishInListView;

            _dispatcherTimer.Tick += OnScrollChange;
            _dispatcherTimer.Interval = new TimeSpan(0, 0, 0,0,100);
            _dispatcherTimer.Start();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            ZwiggyNotification.CartUpdated -= _dishVM.UpdateDishInListView;
            _dispatcherTimer.Stop();
        }


        private async void OnAddDishClicked(object sender, RoutedEventArgs e)
        {
            DishBObj dish = ((sender as Button).DataContext as DishBObj);

            if(_dishVM.IsSameRestaurantInCart(dish))
            {
                _dishVM.AddDishToCart(dish);
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

                ContentDialog contentDialog = new EditCartContentDialog();
                ContentDialogResult result = await contentDialog.ShowAsync();

                if(result == ContentDialogResult.Secondary)
                {
                    _dishVM.ClearCart();
                    _dishVM.AddDishToCart(dish);
                }

                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
        }

        private void OnRemoveDishClicked(object sender, RoutedEventArgs e)
        {
            DishBObj dish = ((sender as Button).DataContext as DishBObj);
            _dishVM.RemoveDishFromCart(dish);
        }

        private async void OnDishHeaderClicked(object sender, RoutedEventArgs e)
        {
            string dishCategory = (((sender as Button).Content as Grid).Children[0] as TextBlock).Text;
            int categoryIndex = -1;
            IList<DishCollection> dishCollections = _dishVM.DishCVS.Source as ObservableCollection<DishCollection>;

            for(int i=0;i<dishCollections.Count;i++)
            {
                if(dishCategory.Equals(dishCollections[i].Key))
                {
                    categoryIndex = i;
                    dishCollections[i].ShowDish = !dishCollections[i].ShowDish;
                    break;
                }
            }

            await _dishVM.GetDishesAsync(dishCollections);
            dishCollections = _dishVM.DishCVS.Source as ObservableCollection<DishCollection>;

            if (dishCollections[dishCollections.Count-1].Count > 0)
            {
                DishListView.ScrollIntoView(dishCollections[dishCollections.Count - 1][dishCollections[dishCollections.Count - 1].Count - 1]);
            }
            else
            {
                DishListView.ScrollIntoView(dishCollections[dishCollections.Count-1]);
            }

            DishListView.ScrollIntoView(dishCollections[categoryIndex]);
        }

        private void OnScrollChange(object sender, object e)
        {
            Border border = VisualTreeHelper.GetChild((DishListView), 0) as Border;
            ScrollViewer scrollViewer = border.Child as ScrollViewer;

            if (scrollViewer != null)
            {
                int index = (int)Math.Round(scrollViewer.VerticalOffset / 200);
                DishBObj dish = _dishVM.DishCVS.View[index] as DishBObj;

                if (dish == null)
                {
                    DishCategoryListView.SelectedIndex = 0;
                    return;
                }

                ObservableCollection<string> dishCategories = DishCategoryListView.ItemsSource as ObservableCollection<string>;

                for (int i = 0; i < dishCategories.Count; i++)
                {
                    if (dishCategories[i].Equals(dish.Category.Category))
                    {
                        DishCategoryListView.SelectedIndex = i;
                        break;
                    }
                }

                if (scrollViewer.VerticalOffset >= (scrollViewer.ScrollableHeight - 50))
                {
                    DishCategoryListView.SelectedIndex = dishCategories.Count - 1;
                }
            }
        }

        private void OnScrollViewChange(object se, ScrollViewerViewChangingEventArgs ev)
        {
            ScrollViewer scrollViewer = se as ScrollViewer;
            if (scrollViewer != null)
            {
                int index = (int)Math.Round(scrollViewer.VerticalOffset / 200);
                DishBObj dish = _dishVM.DishCVS.View[index] as DishBObj;

                if (dish == null)
                {
                    DishCategoryListView.SelectedIndex = 0;
                    return;
                }

                ObservableCollection<string> dishCategories = DishCategoryListView.ItemsSource as ObservableCollection<string>;

                for (int i = 0; i < dishCategories.Count; i++)
                {
                    if (dishCategories[i].Equals(dish.Category.Category))
                    {
                        DishCategoryListView.SelectedIndex = i;
                        break;
                    }
                }

                if (scrollViewer.VerticalOffset >= (scrollViewer.ScrollableHeight-75))
                {
                    DishCategoryListView.SelectedIndex = dishCategories.Count - 1;
                }
            }
        }

        private void DishListView_Loaded(object sender, RoutedEventArgs e)
        {
            Border border = VisualTreeHelper.GetChild((sender as ListView), 0) as Border;
            ScrollViewer scrollViewer = border.Child as ScrollViewer;

            scrollViewer.ViewChanging += OnScrollViewChange;

        }

        private void GetCartView(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CartView));
        }

        private void OnFavouriteButtonClicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Grid grid = button.Content as Grid;
            TextBlock textBlock = grid.Children[0] as TextBlock;

            if(!_dishVM.UpdateFavouriteRestauarnt())
            {
                textBlock.Foreground = new SolidColorBrush(Colors.Transparent);
            }
            else
            {
                textBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 254, 1, 0));
            }
        }

        private void OnFavouriteButtonPointerEnter(object sender, PointerRoutedEventArgs e)
        {
            Button button = sender as Button;
            Grid grid = button.Content as Grid;
            TextBlock textBlock = grid.Children[0] as TextBlock;

            if(!_dishVM.RestaurantInView.IsFavourite)
            {
                textBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 172, 172, 172));
            }
        }

        private void OnFavouriteButtonPointerExit(object sender, PointerRoutedEventArgs e)
        {
            Button button = sender as Button;
            Grid grid = button.Content as Grid;
            TextBlock textBlock = grid.Children[0] as TextBlock;

            if(!_dishVM.RestaurantInView.IsFavourite)
            {
                textBlock.Foreground = new SolidColorBrush(Colors.Transparent);
            }
        }

        private async void OnVegButtonClicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Grid grid = button.Content as Grid;
            Grid textBlockContainer = grid.Children[0] as Grid;
            Border border = textBlockContainer.Children[1] as Border;
            TextBlock textBlock = border.Child as TextBlock;

            if(ApplicationData.Current.LocalSettings.Values["VegFilter"].Equals("0"))
            {
                textBlockContainer.HorizontalAlignment = HorizontalAlignment.Right;
                grid.Background = new SolidColorBrush(Color.FromArgb(255,0,128,0));
                textBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 128, 0));

                await _dishVM.GetDishesAsync(true);

                DishCategoryListView.SelectedIndex = 0;
                ApplicationData.Current.LocalSettings.Values["VegFilter"] = "1";
            }
            else
            {
                textBlockContainer.HorizontalAlignment = HorizontalAlignment.Left;
                grid.Background = new SolidColorBrush(Color.FromArgb(255, 172, 172, 172));
                textBlock.Foreground = new SolidColorBrush(Colors.Transparent);

                await _dishVM.GetDishesAsync(false);

                DishCategoryListView.SelectedIndex = 0;
                ApplicationData.Current.LocalSettings.Values["VegFilter"] = "0";
            }
        }

        private void OnDishCategoryClicked(object sender, ItemClickEventArgs e)
        {
            Border border = VisualTreeHelper.GetChild((DishListView), 0) as Border;
            ScrollViewer scrollViewer = border.Child as ScrollViewer;

            scrollViewer.ViewChanging -= OnScrollViewChange;

            string selectedDishCategory = e.ClickedItem as string;
            ObservableCollection<DishCollection> dishCollections = _dishVM.DishCVS.Source as ObservableCollection<DishCollection>;

            for (int i = 0; i < dishCollections.Count; i++)
            {
                if (dishCollections[i].Key.Equals(selectedDishCategory))
                {
                    if (dishCollections[dishCollections.Count - 1].ShowDish)
                    {
                        DishListView.ScrollIntoView(dishCollections[dishCollections.Count - 1][dishCollections[dishCollections.Count - 1].Count - 1]);
                    }
                    else
                    {
                        DishListView.ScrollIntoView(dishCollections[dishCollections.Count - 1]);
                    }
                    DishListView.ScrollIntoView(dishCollections[i]);
                    break;
                }
            }

            scrollViewer.ViewChanging += OnScrollViewChange;
        }

        private async void OnAddDishClicked(DishBObj dish)
        {
            if (_dishVM.IsSameRestaurantInCart(dish))
            {
                _dishVM.AddDishToCart(dish);
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

                ContentDialog contentDialog = new EditCartContentDialog();
                ContentDialogResult result = await contentDialog.ShowAsync();

                if (result == ContentDialogResult.Secondary)
                {
                    _dishVM.ClearCart();
                    _dishVM.AddDishToCart(dish);
                }

                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
        }

        private void OnRemoveDishClicked(DishBObj dish)
        {
            _dishVM.RemoveDishFromCart(dish);
        }
    }
}
