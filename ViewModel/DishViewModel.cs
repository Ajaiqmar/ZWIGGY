using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Zwiggy.AppData;
using Zwiggy.Core.DataManager;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Notifications;
using Zwiggy.Core.Usecases.AddToCart;
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.Usecases.GetCart;
using Zwiggy.Core.Utility;
using Zwiggy.Core.ZwiggyEventArgs;
using Zwiggy.View.Contract;
using Zwiggy.ViewModel.Contract;
using Zwiggy.ViewObj;

namespace Zwiggy.ViewModel
{
    class DishViewModel : DishViewModelBase
    {
        private IDishView _view;

        public DishViewModel(IDishView view)
        {
            _view = view;
        }

        private FetchDishesDataManager _fetchDishesDataManager = new FetchDishesDataManager();
        private FetchRestaurantDataManager _fetchRestaurantDataManager = new FetchRestaurantDataManager();
        private UpdateRestaurantDataManager _updateRestaurantDataManager = new UpdateRestaurantDataManager();
        private FetchCartDataManager _fetchCartDataManager = new FetchCartDataManager();
        private UpdateCartDataManager _updateCartDataManager = new UpdateCartDataManager();

        private class AddToCartPresenterCallback : IPresenterCallback<AddToCartResponseObj>
        {
            private DishViewModel _dishViewModel;

            public AddToCartPresenterCallback(DishViewModel dishViewModel)
            {
                _dishViewModel = dishViewModel;
            }

            public async Task OnSuccess(AddToCartResponseObj responseObj)
            {
                await _dishViewModel._view.GetDispatcher().RunAsync(CoreDispatcherPriority.Normal, () =>
                 {
                     _dishViewModel.UpdateDishInCart(responseObj.DishObj,true);
                 });
            }

            public void OnError(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private class GetCartPresenterCallback : IPresenterCallback<GetCartResponseObj>
        {
            private DishViewModel _dishViewModel;

            public GetCartPresenterCallback(DishViewModel dishViewModel)
            {
                _dishViewModel = dishViewModel;
            }

            public async Task OnSuccess(GetCartResponseObj responseObj)
            {
                await _dishViewModel._view.GetDispatcher().RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    _dishViewModel.SetCart(responseObj);
                });
            }

            public void OnError(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void UpdateDishInCart(DishBObj dishObj,bool isAdded)
        {
            if(IsCartEmpty)
            {
                IsCartEmpty = false;

                ObservableCollection<DishBObj> dishes = new ObservableCollection<DishBObj>();
                dishes.Add(dishObj);

                CartBObj cart = new CartBObj()
                {
                    RestaurantPicked = RestaurantInView,
                    Dishes = dishes,
                    BillAmount = dishObj.Cost
                };

                CartInView = cart;
            }
            else if(dishObj.DishCount == 0)
            {
                for (int i = 0; i < _cart.Dishes.Count; i++)
                {
                    if (_cart.Dishes[i].Id.Equals(dishObj.Id))
                    {
                        _cart.Dishes.RemoveAt(i);
                        _cart.BillAmount -= dishObj.Cost;
                        break;
                    }
                }

                if(_cart.Dishes.Count == 0)
                {
                    IsCartEmpty = true;
                }
            }
            else if(isAdded && dishObj.DishCount == 1)
            {
                _cart.Dishes.Add(dishObj);
                _cart.BillAmount += dishObj.Cost;
            }
            else
            {
                for (int i = 0; i < _cart.Dishes.Count; i++)
                {
                    if (_cart.Dishes[i].Id.Equals(dishObj.Id))
                    {
                        _cart.Dishes[i].DishCount = dishObj.DishCount;

                        if(isAdded)
                        {
                            _cart.BillAmount += dishObj.Cost;
                        }
                        else
                        {
                            _cart.BillAmount -= dishObj.Cost;
                        }
                    }
                }
            }
        }

        public async override void UpdateDishInListView(UpdateCartEventArgs e)
        {
            CoreDispatcher dispatcher = CoreApplication.MainView.Dispatcher;

            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (e.UpdatedDish != null)
                {
                    bool hasUpdated = false;

                    foreach (DishCollection dishCollection in (DishCVS.Source as ObservableCollection<DishCollection>))
                    {
                        foreach (DishBObj dish in dishCollection)
                        {
                            if (dish.Id.Equals(e.UpdatedDish.Id))
                            {
                                dish.DishCount = e.UpdatedDish.DishCount;
                                hasUpdated = true;
                                break;
                            }
                        }

                        if (hasUpdated)
                        {
                            break;
                        }
                    }
                }
            });
        }

        public override async Task IsFavouriteRestaurantAsync()
        {
            await _fetchRestaurantDataManager.IsFavouriteRestaurantAsync(Repository.CurrentUser, RestaurantInView);
        }

        public override async Task GetDishesAsync()
        {
            IDictionary<string, IList<DishBObj>> dishes = await _fetchDishesDataManager.GetDishesAsync(RestaurantInView,Repository.CurrentUser);
            ObservableCollection<DishCollection> dishCollections = new ObservableCollection<DishCollection>();

            RestaurantDishCategories = new ObservableCollection<string>(dishes.Keys);

            foreach (string dishCategory in dishes.Keys)
            {
                DishCollection dishCollection = new DishCollection(dishes[dishCategory]);
                dishCollection.Key = dishCategory;

                dishCollections.Add(dishCollection);
            }

            CollectionViewSource dishCVS = new CollectionViewSource();
            dishCVS.IsSourceGrouped = true;
            dishCVS.Source = dishCollections;

            DishCVS = dishCVS;
        }

        public override async Task GetDishesAsync(IList<DishCollection> dishCollections)
        {
            IDictionary<string, bool> dishCategoriesVisibility = new Dictionary<string, bool>();

            foreach (DishCollection dishCollection in dishCollections)
            {
                dishCategoriesVisibility[dishCollection.Key] = dishCollection.ShowDish;
            }

            IDictionary<string, IList<DishBObj>> dishes = await _fetchDishesDataManager.GetDishesAsync(RestaurantInView, dishCategoriesVisibility,Repository.CurrentUser);
            ObservableCollection<DishCollection> newDishCollections = new ObservableCollection<DishCollection>();

            foreach (string category in dishes.Keys)
            {
                newDishCollections.Add(new DishCollection(dishes[category]) { Key = category, ShowDish = dishCategoriesVisibility[category] });

            }

            CollectionViewSource dishCVS = new CollectionViewSource();
            dishCVS.IsSourceGrouped = true;
            dishCVS.Source = newDishCollections;

            DishCVS = dishCVS;
        }

        public override async Task GetDishesAsync(bool isVeg)
        {
            IDictionary<string, IList<DishBObj>> dishes = await _fetchDishesDataManager.GetDishesAsync(RestaurantInView, isVeg,Repository.CurrentUser);
            ObservableCollection<DishCollection> dishCollections = new ObservableCollection<DishCollection>();

            RestaurantDishCategories = new ObservableCollection<string>(dishes.Keys);

            foreach (string dishCategory in dishes.Keys)
            {
                DishCollection dishCollection = new DishCollection(dishes[dishCategory]);
                dishCollection.Key = dishCategory;

                dishCollections.Add(dishCollection);
            }

            CollectionViewSource dishCVS = new CollectionViewSource();
            dishCVS.IsSourceGrouped = true;
            dishCVS.Source = dishCollections;

            DishCVS = dishCVS;
        }

        // CLEAN ARCHITECTURE - GET CART
        public override void GetCart()
        {
            GetCartRequestObj requestObj = new GetCartRequestObj() { UserObj = Repository.CurrentUser };
            GetCart usecase = new GetCart(new GetCartPresenterCallback(this),requestObj);

            usecase.Execute();
        }

        public void SetCart(GetCartResponseObj responseObj)
        {
            CartBObj cart = responseObj.Cart;

            if (cart.Dishes.Count == 0)
            {
                IsCartEmpty = true;
                CartInView = null;
            }
            else
            {
                IsCartEmpty = false;
                CartInView = cart;
            }
        }

        // CLEAN ARCHITECTURE - ADD TO CART
        public override void AddDishToCart(DishBObj dish)
        {
            AddToCartRequestObj requestObj = new AddToCartRequestObj() { DishObj = dish, UserObj = Repository.CurrentUser };
            AddToCart usecase = new AddToCart(new AddToCartPresenterCallback(this),requestObj);

            dish.DishCount += 1;

            if(IsCartEmpty)
            {
                usecase.Execute();
            }
            else
            {
                if(dish.DishCount == 1)
                {
                    usecase.Execute();
                    return;
                }

                _updateCartDataManager.UpdateDishCountInCart(dish, Repository.CurrentUser);
                UpdateDishInCart(dish,true);
            }
        }

        public override void RemoveDishFromCart(DishBObj dish)
        {
            dish.DishCount -= 1;

            if (dish.DishCount == 0)
            {
                _updateCartDataManager.RemoveDishFromCart(dish, Repository.CurrentUser);
                UpdateDishInCart(dish,false);
            }
            else
            {
                _updateCartDataManager.UpdateDishCountInCart(dish, Repository.CurrentUser);
                UpdateDishInCart(dish,false);
            }
        }

        public override void ClearCart()
        {
            _updateCartDataManager.ClearCart(Repository.CurrentUser);
            IsCartEmpty = true;
        }

        public override bool UpdateFavouriteRestauarnt()
        {
            if (RestaurantInView.IsFavourite)
            {
                _updateRestaurantDataManager.RemoveFavouriteRestaurant(Repository.CurrentUser, RestaurantInView);
            }
            else
            {
                _updateRestaurantDataManager.AddFavouriteRestaurant(Repository.CurrentUser, RestaurantInView);
            }

            RestaurantInView.IsFavourite = !RestaurantInView.IsFavourite;

            return RestaurantInView.IsFavourite;
        }

        public override bool IsSameRestaurantInCart(DishBObj dish)
        {
            if (IsCartNotEmpty)
            {
                return _cart.RestaurantPicked.Id.Equals(dish.RestaurantId);
            }

            return true;
        }
    }
}
