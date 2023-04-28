using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Zwiggy.AppData;
using Zwiggy.Core.Model;
using Zwiggy.Core.ModelBObj;
using Zwiggy.DependencyService;
using Zwiggy.View.Contract;
using Zwiggy.ViewModel;
using Zwiggy.ViewModel.Contract;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Zwiggy.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CartView : Page,ICartView
    {
        private CartViewModelBase _cartVM;

        public CartView()
        {
            this.InitializeComponent();
            _cartVM = (CartViewModelBase)ActivatorUtilities.CreateInstance<CartViewModel>(UIDependencyInjector.ServiceProvider,this);
        }

        public CoreDispatcher GetDispatcher()
        {
            return this.Dispatcher;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _cartVM.GetAddress();
            _cartVM.GetCart();
        }

        private void OnAddAddressButtonClicked(object sender, RoutedEventArgs e)
        {
            AddressContentOverlay.Visibility = Visibility.Visible;
            AddressSplitView.IsPaneOpen = true;
        }

        private void OnAddressPaneExitButtonClicked(object sender, RoutedEventArgs e)
        {
            AddressContentOverlay.Visibility = Visibility.Collapsed;
            AddressSplitView.IsPaneOpen = false;
        }

        private void OnAddressSplitViewPaneClosed(SplitView sender, object args)
        {
            AddressContentOverlay.Visibility = Visibility.Collapsed;
        }

        private async void OnAddressSplitViewPaneOpened(SplitView sender, object args)
        {
            GeolocationAccessStatus result = await Geolocator.RequestAccessAsync();

            if (result == GeolocationAccessStatus.Allowed)
            {
                var myLandmarks = new List<MapElement>();

                Geolocator geolocator = new Geolocator();
                Geoposition pos = await geolocator.GetGeopositionAsync();
                Geopoint myLocation = pos.Coordinate.Point;

                var mapNeedleIcon = new MapIcon
                {
                    Location = myLocation,
                    NormalizedAnchorPoint = new Point(0.5, 1.0),
                    ZIndex = 0,
                    Title = "Your Location"
                };

                myLandmarks.Add(mapNeedleIcon);

                var landmarksLayer = new MapElementsLayer
                {
                    ZIndex = 1,
                    MapElements = myLandmarks
                };

                AddressMap.Layers.Add(landmarksLayer);
                AddressMap.Center = myLocation;
                AddressMap.ZoomLevel = 16;
                AddressMap.LandmarksVisible = true;
            }
        }

        private void OnAddressFormSubmitted(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AddressTextBox.Text) && string.IsNullOrWhiteSpace(AddressNicknameTextBox.Text))
            {
                AddressTextBox.Background = new SolidColorBrush(Color.FromArgb(255, 248, 196, 180));
                AddressNicknameTextBox.Background = new SolidColorBrush(Color.FromArgb(255, 248, 196, 180));
            }
            else if (string.IsNullOrWhiteSpace(AddressTextBox.Text))
            {
                AddressTextBox.Background = new SolidColorBrush(Color.FromArgb(255, 248, 196, 180));
            }
            else if (string.IsNullOrWhiteSpace(AddressNicknameTextBox.Text))
            {
                AddressNicknameTextBox.Background = new SolidColorBrush(Color.FromArgb(255, 248, 196, 180));
            }
            else
            {
                _cartVM.AddAddress();
                AddressTextBox.Text = "";
                AddressNicknameTextBox.Text = "";
                AddressSplitView.IsPaneOpen = false;
            }

        }

        private void OnTextBoxPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.Background = new SolidColorBrush(Colors.White);
        }

        private void OnAddressSelected(object sender, RoutedEventArgs e)
        {
            if (_cartVM.IsCartEmpty)
            {
                CartEmptyTeachingTip.IsOpen = true;
                return;
            }

            Button button = sender as Button;
            Address address = button.DataContext as Address;

            _cartVM.AddressSelected = address;

            AddressSection.Height = 140;
            AddressGridView.Visibility = Visibility.Collapsed;
            AddressCheckBorder.Visibility = Visibility.Visible;
            SelectedAddressDetails.Visibility = Visibility.Visible;
            SelectedAddressNickname.Text = address.Nickname;
            SelectedAddressDescription.Text = address.AddressDescription;
            AddressHeader.Text = "Delivery Location";
            AddressHeaderSection.Margin = new Thickness(30, 30, 30, 10);
            CheckoutFirstRow.Height = GridLength.Auto;
            CheckoutSecondRow.Height = new GridLength(1, GridUnitType.Star);
            AddAddressButton.Visibility = Visibility.Collapsed;
            AddressChangeButton.Visibility = Visibility.Visible;
            PaymentSectionGrid.Visibility = Visibility.Visible;
        }

        private void OnChangeAddressRequested(object sender, PointerRoutedEventArgs e)
        {
            if (AddressCheckBorder.Visibility == Visibility.Visible && PaymentCheckBorder.Visibility == Visibility.Collapsed)
            {
                AddressSection.Height = double.NaN;
                AddressGridView.Visibility = Visibility.Visible;
                AddressCheckBorder.Visibility = Visibility.Collapsed;
                SelectedAddressDetails.Visibility = Visibility.Collapsed;
                SelectedAddressNickname.Text = "";
                SelectedAddressDescription.Text = "";
                AddressHeader.Text = "Pick a Delivery Location";
                AddressHeaderSection.Margin = new Thickness(30);
                CheckoutFirstRow.Height = new GridLength(1, GridUnitType.Star);
                CheckoutSecondRow.Height = new GridLength(100, GridUnitType.Pixel);
                AddAddressButton.Visibility = Visibility.Visible;
                AddressChangeButton.Visibility = Visibility.Collapsed;
                PaymentSectionGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void OnChangeAddressRequested(object sender, RoutedEventArgs e)
        {
            if (AddressCheckBorder.Visibility == Visibility.Visible && PaymentCheckBorder.Visibility == Visibility.Collapsed)
            {
                AddressSection.Height = double.NaN;
                AddressGridView.Visibility = Visibility.Visible;
                AddressCheckBorder.Visibility = Visibility.Collapsed;
                SelectedAddressDetails.Visibility = Visibility.Collapsed;
                SelectedAddressNickname.Text = "";
                SelectedAddressDescription.Text = "";
                AddressHeader.Text = "Pick a Delivery Location";
                AddressHeaderSection.Margin = new Thickness(30);
                CheckoutFirstRow.Height = new GridLength(1, GridUnitType.Star);
                CheckoutSecondRow.Height = new GridLength(100, GridUnitType.Pixel);
                AddAddressButton.Visibility = Visibility.Visible;
                AddressChangeButton.Visibility = Visibility.Collapsed;
                PaymentSectionGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void OnRemoveDishClicked(DishBObj dish)
        {
            _cartVM.RemoveFromCart(dish);
        }

        private void OnAddDishClicked(DishBObj dish)
        {
            _cartVM.AddToCart(dish);
        }

        private void OnOrderPlaced(object sender, RoutedEventArgs e)
        {
            if(_cartVM.IsCartEmpty)
            {
                return;
            }

            PaymentSectionGrid.Visibility = Visibility.Collapsed;
            SelectedPaymentDetails.Visibility = Visibility.Visible;
            CheckoutSecondRow.Height = GridLength.Auto;
            CheckoutThirdRow.Height = new GridLength(1, GridUnitType.Star);
            PaymentSectionHeader.Text = "Payment Mode";
            PaymentCheckBorder.Visibility = Visibility.Visible;
            AddressChangeButton.Visibility = Visibility.Collapsed;
            _cartVM.IsCartEmpty = true;

            if (PayOnDeliveryListItem.IsSelected)
            {
                SelectedPaymentMode.Text = "Pay on Delivery";
                _cartVM.ModeOfPayment = Payment.PayOnDelivery;
            }
            else if (UPIListItem.IsSelected)
            {
                SelectedPaymentMode.Text = "UPI";
                _cartVM.ModeOfPayment = Payment.UPI;
            }
            else if (CardListItem.IsSelected)
            {
                SelectedPaymentMode.Text = "Credit & Debit Card";
                _cartVM.ModeOfPayment = Payment.Card;
            }

            RatingSectionGrid.Visibility = Visibility.Visible;

            _cartVM.PlaceOrder();
        }



        private void OnModeOfPaymentSelected(object sender, SelectionChangedEventArgs e)
        {
            if (PayOnDeliveryListItem.IsSelected)
            {
                PayOnDeliveryContentPanel.Visibility = Visibility.Visible;
                UPIContentPanel.Visibility = Visibility.Collapsed;
                CardContentPanel.Visibility = Visibility.Collapsed;
            }
            else if (UPIListItem.IsSelected)
            {
                PayOnDeliveryContentPanel.Visibility = Visibility.Collapsed;
                UPIContentPanel.Visibility = Visibility.Visible;
                CardContentPanel.Visibility = Visibility.Collapsed;
            }
            else if (CardListItem.IsSelected)
            {
                PayOnDeliveryContentPanel.Visibility = Visibility.Collapsed;
                UPIContentPanel.Visibility = Visibility.Collapsed;
                CardContentPanel.Visibility = Visibility.Visible;
            }
        }
    }
}
