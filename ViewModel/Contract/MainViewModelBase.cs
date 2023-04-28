using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.DataManager;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Utility;

namespace Zwiggy.ViewModel.Contract
{
    abstract class MainViewModelBase : BindingsNotification
    {
        private bool _cartSelected = false;
        private bool _cartViewRequested = false;
        private bool _homeSelected = true;
        private string _searchText = "";
        private double _popupWidth = 0.0;
        private bool _isInsideSearch = false;
        private bool _isSearchEmpty = true;
        private bool _isRestaurantAvailable = false;
        private bool _isDishAvailable = false;
        private ObservableCollection<RestaurantBObj> _restaurants = null;
        protected FetchCartDataManager FetchCartDataManager = new FetchCartDataManager();

        public string SearchText
        {
            get
            {
                return _searchText;
            }

            set
            {
                _searchText = value;
                OnPropertyChange(nameof(SearchText));
            }
        }

        public bool CartSelected
        {
            get
            {
                return _cartSelected;
            }

            set
            {
                _cartSelected = value;
                OnPropertyChange(nameof(CartSelected));
            }
        }

        public bool CartViewRequested
        {
            get
            {
                return _cartViewRequested;
            }

            set
            {
                _cartViewRequested = value;
                OnPropertyChange(nameof(CartViewRequested));
            }
        }

        public bool HomeSelected
        {
            get
            {
                return _homeSelected;
            }

            set
            {
                _homeSelected = value;
                OnPropertyChange(nameof(HomeSelected));
            }
        }

        public double PopupWidth
        {
            get
            {
                return _popupWidth;
            }

            set
            {
                _popupWidth = value;
                OnPropertyChange(nameof(PopupWidth));
            }
        }

        public bool IsInsideSearch
        {
            get
            {
                return _isInsideSearch;
            }

            set
            {
                _isInsideSearch = value;
                OnPropertyChange(nameof(IsInsideSearch));
            }
        }

        public bool IsSearchEmpty
        {
            get
            {
                return _isSearchEmpty;
            }

            set
            {
                _isSearchEmpty = value;
                OnPropertyChange(nameof(IsSearchEmpty));
            }
        }

        public bool IsRestaurantAvailable
        {
            get
            {
                return _isRestaurantAvailable;
            }

            set
            {
                _isRestaurantAvailable = value;
                OnPropertyChange(nameof(IsRestaurantAvailable));
            }
        }

        public bool IsDishAvailable
        {
            get
            {
                return _isDishAvailable;
            }

            set
            {
                _isDishAvailable = value;
                OnPropertyChange(nameof(IsDishAvailable));
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

        public abstract Task<long> GetCartDishesCountAsync();

        public abstract void GetRestaurantSearchResults();

        public abstract void GetDishSearchResults();
    }
}
