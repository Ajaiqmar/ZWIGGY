using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Zwiggy.Core.Model;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Utility;
using Zwiggy.ViewObj;

namespace Zwiggy.ViewModel.Contract
{
    abstract class ProfileViewModelBase : BindingsNotification
    {
        private IList<ProfileSections> _sections;
        private ObservableCollection<RestaurantBObj> _restaurants = null;
        private ObservableCollection<Address> _addresses = null;
        private ObservableCollection<OrderBObj> _orders = null;
        private OrderBObj _orderDetails = null;

        private bool _isOrderEmpty = false;
        private bool _isFavouritesEmpty = false;
        private bool _isAddressEmpty = false;
        private bool _isOrderNotEmpty = false;
        private bool _isFavouritesNotEmpty = false;
        private bool _isAddressNotEmpty = false;
        private bool _isSettingsVisible = false;

        public IList<ProfileSections> Sections 
        {
            get
            {
                return _sections;
            }
            set
            {
                _sections = value;
                OnPropertyChange(nameof(Sections));
            }
        }

        public ObservableCollection<RestaurantBObj> Restaurants
        {
            get
            {
                return _restaurants;
            }

            set
            {
                _restaurants = value;
                OnPropertyChange(nameof(Restaurants));
            }
        }

        public bool IsAddressEmpty
        {
            get
            {
                return _isAddressEmpty;
            }

            set
            {
                _isAddressEmpty = value;
                OnPropertyChange(nameof(IsAddressEmpty));
            }
        }

        public bool IsAddressNotEmpty
        {
            get
            {
                return _isAddressNotEmpty;
            }

            set
            {
                _isAddressNotEmpty = value;
                OnPropertyChange(nameof(IsAddressNotEmpty));
            }
        }
        public bool IsFavouritesEmpty
        {
            get
            {
                return _isFavouritesEmpty;
            }

            set
            {
                _isFavouritesEmpty = value;
                OnPropertyChange(nameof(IsFavouritesEmpty));
            }
        }

        public bool IsFavouritesNotEmpty
        {
            get
            {
                return _isFavouritesNotEmpty;
            }

            set
            {
                _isFavouritesNotEmpty = value;
                OnPropertyChange(nameof(IsFavouritesNotEmpty));
            }
        }
        public bool IsOrderEmpty
        {
            get
            {
                return _isOrderEmpty;
            }

            set
            {
                _isOrderEmpty = value;
                OnPropertyChange(nameof(IsOrderEmpty));
            }
        }

        public bool IsOrderNotEmpty
        {
            get
            {
                return _isOrderNotEmpty;
            }

            set
            {
                _isOrderNotEmpty = value;
                OnPropertyChange(nameof(IsOrderNotEmpty));
            }
        }

        public bool IsSettingsVisible
        {
            get
            {
                return _isSettingsVisible;
            }

            set
            {
                _isSettingsVisible = value;
                OnPropertyChange(nameof(IsSettingsVisible));
            }
        }

        public ObservableCollection<Address> Addresses
        {
            get
            {
                return _addresses;
            }

            set
            {
                _addresses = value;
                OnPropertyChange(nameof(Addresses));
            }
        }

        public ObservableCollection<OrderBObj> Orders
        {
            get
            {
                return _orders;
            }

            set
            {
                _orders = value;
                OnPropertyChange(nameof(Orders));
            }
        }

        public OrderBObj OrderDetails
        {
            get
            {
                return _orderDetails;
            }

            set
            {
                _orderDetails = value;
                OnPropertyChange(nameof(OrderDetails));
            }
        }

        public TextBlock SelectedAccentColorTextBlock{ get; set; }

        public abstract void GetFavouriteRestaurants();

        public abstract void GetAddress();

        public abstract void GetOrders();

        public abstract void GetOrderDetails(OrderBObj order);

        public abstract void RemoveAddress(Address address);
    }
}
