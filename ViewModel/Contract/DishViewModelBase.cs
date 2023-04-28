using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Utility;
using Zwiggy.Core.ZwiggyEventArgs;
using Zwiggy.ViewObj;

namespace Zwiggy.ViewModel.Contract
{
    abstract class DishViewModelBase : BindingsNotification
    {
        private bool _isCartEmpty = true;
        private RestaurantBObj _restaurantInView = null;
        private ObservableCollection<string> _restaurantDishCategories = null;
        private CollectionViewSource _dishCVS = null;
        protected CartBObj _cart = null;

        public bool IsCartEmpty
        {
            get
            {
                return _isCartEmpty;
            }

            set
            {
                _isCartEmpty = value;
                OnPropertyChange(nameof(IsCartEmpty));
                OnPropertyChange(nameof(IsCartNotEmpty));
            }
        }

        public RestaurantBObj RestaurantInView
        {
            get
            {
                return _restaurantInView;
            }

            set
            {
                _restaurantInView = value;
            }
        }

        public ICollectionView Dishes
        {
            get
            {
                return _dishCVS.View;
            }
        }

        public CollectionViewSource DishCVS
        {
            get
            {
                return _dishCVS;
            }

            set
            {
                _dishCVS = value;
                OnPropertyChange(nameof(Dishes));
            }
        }

        public ObservableCollection<string> RestaurantDishCategories
        {
            get
            {
                return _restaurantDishCategories;
            }

            set
            {
                _restaurantDishCategories = value;
                OnPropertyChange(nameof(RestaurantDishCategories));
            }
        }

        public bool IsCartNotEmpty
        {
            get
            {
                return !_isCartEmpty;
            }
        }

        public CartBObj CartInView
        {
            set
            {
                _cart = value;
                OnPropertyChange(nameof(CartInView));
            }

            get
            {
                return _cart;
            }
        }

        public abstract Task IsFavouriteRestaurantAsync();
        public abstract void GetCart();
        public abstract Task GetDishesAsync();
        public abstract Task GetDishesAsync(bool isVeg);
        public abstract Task GetDishesAsync(IList<DishCollection> dishCollections);
        public abstract bool IsSameRestaurantInCart(DishBObj dish);
        public abstract void ClearCart();
        public abstract void AddDishToCart(DishBObj dish);
        public abstract void RemoveDishFromCart(DishBObj dish);
        public abstract bool UpdateFavouriteRestauarnt();
        public abstract void UpdateDishInListView(UpdateCartEventArgs e);
    }
}
