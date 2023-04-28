using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Zwiggy.Core.Notifications;
using Zwiggy.Core.ZwiggyEventArgs;
using Zwiggy.Service;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Zwiggy.View.User_Control
{
    public sealed partial class AppNavigationControl : UserControl
    {
        public event EventHandler OnHomeSelected;
        public event EventHandler OnProfileSelected;
        public event EventHandler OnCartSelected;
        public event Func<Task<long>> GetDishesCount;
        public event EventHandler LogOut;

        public static readonly DependencyProperty CartSelectedProperty = DependencyProperty.Register("CartSelected",typeof(bool),typeof(AppNavigationControl),new PropertyMetadata(null));

        public static readonly DependencyProperty CartViewRequestedProperty = DependencyProperty.Register("CartViewRequested",typeof(bool),typeof(AppNavigationControl),new PropertyMetadata(null));
        
        public static readonly DependencyProperty HomeSelectedProperty = DependencyProperty.Register("HomeSelected",typeof(bool),typeof(AppNavigationControl),new PropertyMetadata(null));
        
        public static readonly DependencyProperty CartDishCountProperty = DependencyProperty.Register("CartDishCount",typeof(long),typeof(AppNavigationControl),new PropertyMetadata(null));


        public bool CartSelected
        {
            get { return (bool)GetValue(CartSelectedProperty); }
            set { SetValue(CartSelectedProperty, value); }
        }
        
        public bool CartViewRequested
        {
            get { return (bool)GetValue(CartViewRequestedProperty); }
            set { SetValue(CartViewRequestedProperty, value); }
        }
        
        public bool HomeSelected
        {
            get { return (bool)GetValue(HomeSelectedProperty); }
            set { SetValue(HomeSelectedProperty, value); }
        }

        public long CartDishCount
        {
            get { return (long)GetValue(CartDishCountProperty); }
            set { SetValue(CartDishCountProperty, value); }
        }

        public AppNavigationControl()
        {
            this.InitializeComponent();
            ZwiggyNotification.CartUpdated += UpdateNotificationPopout;
        }

        private void OnNavigationSelected(object sender, SelectionChangedEventArgs e)
        {
            if(CartHighlight.Visibility == Visibility.Visible && CartNavigationItem.IsSelected == false && CartDishCount > 0)
            {
                ToastService.Instance.ShowItemInCartToast(CartNavigationItem);
            }

            ProfileHighlight.Visibility = Visibility.Collapsed;
            CartHighlight.Visibility = Visibility.Collapsed;
            HomeHighlight.Visibility = Visibility.Collapsed;
            CartTextBlock.Foreground = new SolidColorBrush(Colors.White);

            if (ProfileNavigationItem.IsSelected == true)
            {
                ProfileHighlight.Visibility = Visibility.Visible;
                PopupBackground.BorderBrush = new SolidColorBrush(Colors.Transparent);
                PopupBackground.BorderThickness = new Thickness(0);
                OnProfileSelected.Invoke(this,new EventArgs());
            }
            else if(CartNavigationItem.IsSelected == true)
            {
                CartHighlight.Visibility = Visibility.Visible;
                PopupBackground.BorderBrush = Application.Current.Resources["ApplicationBaseAccentColor"] as SolidColorBrush;
                PopupBackground.BorderThickness = new Thickness(1);
                CartTextBlock.Foreground = Application.Current.Resources["ApplicationBaseAccentColor"] as SolidColorBrush;
                OnCartSelected.Invoke(this, new EventArgs());
            }
            else if(HomeNavigationItem.IsSelected == true)
            {
                HomeHighlight.Visibility = Visibility.Visible;
                PopupBackground.BorderBrush = Application.Current.Resources["ApplicationBaseAccentColor"] as SolidColorBrush;
                PopupBackground.BorderThickness = new Thickness(0);

                if (OnHomeSelected != null)
                {
                    OnHomeSelected.Invoke(this, new EventArgs());
                }
            }
        }

        private void OnPointerEnterListItem(object sender, PointerRoutedEventArgs e)
        {
            var listItem = sender as ListBoxItem;

            if(listItem == null)
            {
                CartHighlight.Visibility = Visibility.Visible;
                PopupBackground.BorderBrush = Application.Current.Resources["ApplicationBaseAccentColor"] as SolidColorBrush;
                CartTextBlock.Foreground = Application.Current.Resources["ApplicationBaseAccentColor"] as SolidColorBrush;
                PopupBackground.BorderThickness = new Thickness(1);
            }
            else if(listItem.Name.Equals("ProfileNavigationItem"))
            {
                ProfileHighlight.Visibility = Visibility.Visible;
            }
            else if(listItem.Name.Equals("CartNavigationItem"))
            {
                CartHighlight.Visibility = Visibility.Visible;
                PopupBackground.BorderBrush = Application.Current.Resources["ApplicationBaseAccentColor"] as SolidColorBrush;
                CartTextBlock.Foreground = Application.Current.Resources["ApplicationBaseAccentColor"] as SolidColorBrush;
                PopupBackground.BorderThickness = new Thickness(1);
            }
            else if(listItem.Name.Equals("HomeNavigationItem"))
            {
                HomeHighlight.Visibility = Visibility.Visible;
            }
        }

        private void OnPointerExitListItem(object sender, PointerRoutedEventArgs e)
        {
            var listItem = sender as ListBoxItem;

            if (listItem == null)
            {
                CartHighlight.Visibility = Visibility.Collapsed;
                PopupBackground.BorderBrush = new SolidColorBrush(Colors.Transparent);
                CartTextBlock.Foreground = new SolidColorBrush(Colors.White);
                PopupBackground.BorderThickness = new Thickness(0);
            }
            else if (listItem.Name.Equals("ProfileNavigationItem") && ProfileNavigationItem.IsSelected == false)
            {
                ProfileHighlight.Visibility = Visibility.Collapsed;
            }
            else if(listItem.Name.Equals("CartNavigationItem") && CartNavigationItem.IsSelected == false)
            {
                CartHighlight.Visibility = Visibility.Collapsed;
                PopupBackground.BorderBrush = new SolidColorBrush(Colors.Transparent);
                CartTextBlock.Foreground = new SolidColorBrush(Colors.White);
                PopupBackground.BorderThickness = new Thickness(0);
            }
            else if (listItem.Name.Equals("HomeNavigationItem") && HomeNavigationItem.IsSelected == false)
            {
                HomeHighlight.Visibility = Visibility.Collapsed;
            }
        }

        private void LogOutUser(object sender, RoutedEventArgs e)
        {
            LogOut.Invoke(this,new EventArgs());
        }

        private async void OnAppNavigationControlLoaded(object sender, RoutedEventArgs e)
        {
            CartDishCount = await GetDishesCount?.Invoke();

            if (CartDishCount == 0)
            {
                NotificationPopup.Visibility = Visibility.Collapsed;
            }
        }

        private async void UpdateNotificationPopout(UpdateCartEventArgs e)
        {
            await CoreApplication.MainView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,() =>
            {
                if (e.UpdatedDish == null)
                {
                    NotificationPopup.Visibility = Visibility.Collapsed;
                    CartDishCount = 0;
                    return;
                }

                if (e.DishAdded && e.UpdatedDish.DishCount == 1)
                {
                    if (CartDishCount == 0)
                    {
                        NotificationPopup.Visibility = Visibility.Visible;
                        CartDishCount += 1;
                    }
                    else
                    {
                        CartDishCount += 1;
                    }
                }
                else if (e.UpdatedDish.DishCount == 0)
                {
                    CartDishCount -= 1;

                    if (CartDishCount == 0)
                    {
                        NotificationPopup.Visibility = Visibility.Collapsed;
                    }
                }

            });
        }

        private void OnHomeNavigationLoaded(object sender, RoutedEventArgs e)
        {
            if(CartViewRequested == true)
            {
                CartNavigationItem.IsSelected = true;
            }
            else
            {
                HomeNavigationItem.IsSelected = true;
            }
        }
    }
}
