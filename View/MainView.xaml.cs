using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Zwiggy.AppData;
using Zwiggy.Core.ModelBObj;
using Zwiggy.DependencyService;
using Zwiggy.Service;
using Zwiggy.View.Contract;
using Zwiggy.View.User_Control;
using Zwiggy.ViewModel;
using Zwiggy.ViewModel.Contract;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Zwiggy.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : Page,IMainView
    {
        private DispatcherTimer _dispatcherTimer;
        private MainViewModelBase _mainVM;
        private TextBox _searchTextbox;

        public MainView()
        {
            this.InitializeComponent();

            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Tick += OnSearchRestaurant;
            _dispatcherTimer.Interval = new TimeSpan(0,0,0,2);

            AppearanceService.ChangeAccentColor(ApplicationData.Current.LocalSettings.Values["AccentColorType"] as string);
            
            _mainVM = ActivatorUtilities.CreateInstance<MainViewModel>(UIDependencyInjector.ServiceProvider,this);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if((e.Parameter as string) != null)
            {
                _mainVM.CartViewRequested = true;
            }
            else
            {
                MainViewFrame.Navigate(typeof(RestaurantView));
            }
        }

        private void NavigateToHome(object sender, EventArgs e)
        {
            MainViewFrame.Navigate(typeof(RestaurantView));
        }

        private void NavigateToProfile(object sender, EventArgs e)
        {
            MainViewFrame.Navigate(typeof(ProfileView));
        }

        private void NavigateToCart(object sender, EventArgs e)
        {
            MainViewFrame.Navigate(typeof(CartView));
        }


        private async void LogOutUser(object sender, EventArgs e)
        {
            bool inDishView = false;

            if(SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility == AppViewBackButtonVisibility.Visible)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                inDishView = true;
            }

            ContentDialog contentDialog = new LogoutContentDialog();
            ContentDialogResult result = await contentDialog.ShowAsync();

            if(result == ContentDialogResult.Secondary)
            {
                Frame.Navigate(typeof(IndexView));
                ApplicationData.Current.LocalSettings.Values["UserSessionObj"] = null;
                ApplicationData.Current.LocalSettings.Values["VegFilter"] = null;
                ApplicationData.Current.LocalSettings.Values["AccentColorType"] = null;
            }
            else if(inDishView)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
        }

        private async Task<long> GetCartDishesCount()
        {
            return await _mainVM.GetCartDishesCountAsync();
        }

        private void OnMainFrameNavigated(object sender, NavigationEventArgs e)
        {
            if (e.SourcePageType == typeof(CartView))
            {
                _mainVM.CartSelected = true;
            }
            else if(e.SourcePageType == typeof(DishView))
            {
                _mainVM.HomeSelected = true;
            }
        }

        private void OnTextboxGotFocus(object sender, EventArgs e)
        {
            _searchTextbox = sender as TextBox;
            _mainVM.PopupWidth = MainViewSearchControl.ActualWidth;

            if(!_mainVM.IsInsideSearch)
            {
                _searchTextbox.Background = new SolidColorBrush(Colors.White);
                _searchTextbox.Foreground = new SolidColorBrush(Colors.Black);
                _searchTextbox.PlaceholderForeground = new SolidColorBrush(Colors.Black);
                _dispatcherTimer?.Start();
                SearchPopup.IsOpen = true;

                if ((bool)RestaurantToggleButton.IsChecked)
                {
                    _mainVM.GetRestaurantSearchResults();
                }
                else
                {
                    _mainVM.GetDishSearchResults();
                }
            }
        }

        private void OnTextboxLostFocus(object sender, EventArgs e)
        {
            _searchTextbox = sender as TextBox;

            if(!_mainVM.IsInsideSearch)
            {
                _searchTextbox.Background = new SolidColorBrush(Colors.Transparent);
                _searchTextbox.PlaceholderForeground = new SolidColorBrush(Colors.White);
                SearchPopup.IsOpen = false;
                _dispatcherTimer?.Stop();
                RestaurantToggleButton.IsChecked = true;
                DishToggleButton.IsChecked = false;
            }
            else
            {
                _searchTextbox.Background = new SolidColorBrush(Colors.White);
                _searchTextbox.PlaceholderForeground = new SolidColorBrush(Colors.Black);
            }
        }

        private void OnSearchRestaurant(object sender, object e)
        {
            if((bool)RestaurantToggleButton.IsChecked)
            {
                _mainVM.GetRestaurantSearchResults();
            }
            else
            {
                _mainVM.GetDishSearchResults();
            }
        }

        public CoreDispatcher GetDispatcher()
        {
            return this.Dispatcher;
        }

        private void OnSearchSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _mainVM.PopupWidth = MainViewSearchControl.ActualWidth;
        }

        private void OnFilterTypeSelected(object sender, RoutedEventArgs e)
        {
            if(_searchTextbox != null)
            {
                _searchTextbox.Focus(FocusState.Programmatic);
            }

            ToggleButton selectedButton = sender as ToggleButton;

            DishToggleButton.IsChecked = false;
            RestaurantToggleButton.IsChecked = false;

            selectedButton.IsChecked = true;

            if ((bool)RestaurantToggleButton.IsChecked)
            {
                _mainVM.GetRestaurantSearchResults();
            }
            else
            {
                _mainVM.GetDishSearchResults();
            }
        }

        private void OnPopupEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _mainVM.IsInsideSearch = true;
        }

        private void OnPopupExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _mainVM.IsInsideSearch = false;
        }

        private void OnSearchButtonPressed(object sender, EventArgs e)
        {
            if ((bool)RestaurantToggleButton.IsChecked)
            {
                _mainVM.GetRestaurantSearchResults();
            }
            else
            {
                _mainVM.GetDishSearchResults();
            }
        }

        private void OnRestaurantSelected(RestaurantBObj selectedRestaurant)
        {
            _mainVM.IsInsideSearch = false;
            MainViewFrame.Navigate(typeof(DishView), selectedRestaurant);
            _mainVM.HomeSelected = true;
            //_searchTextbox.Focus(FocusState.Programmatic);
        }
    }
}
