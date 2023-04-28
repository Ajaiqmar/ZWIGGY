using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Zwiggy.Core.DataManager;
using Zwiggy.Core.Model;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.Usecases.GetRestaurant;
using Zwiggy.Core.Utility;

namespace Zwiggy.ViewModel
{
    class RestaurantViewModel : BindingsNotification
    {
        private ObservableCollection<RestaurantBObj> _restaurants = null;
        private long _restaurantCount = 0;
        private FetchRestaurantDataManager _restaurantFetchDataManager = new FetchRestaurantDataManager();

        private class GetRestaurantPresenterCallback : IPresenterCallback<GetRestaurantResponseObj>
        {
            private RestaurantViewModel _restaurantVM;

            public GetRestaurantPresenterCallback(RestaurantViewModel restaurantVM)
            {
                _restaurantVM = restaurantVM;
            }

            public void OnError(Exception ex)
            {
                Debug.WriteLine(ex);
            }

            public async Task OnSuccess(GetRestaurantResponseObj responseObj)
            {
                CoreDispatcher dispatcher = CoreApplication.MainView.Dispatcher;

                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    _restaurantVM.Restaurants = new ObservableCollection<RestaurantBObj>(responseObj.Restaurants);
                });
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

        public long RestaurantCount
        {
            get
            {
                return _restaurantCount;
            }

            set
            {
                _restaurantCount = value;
                OnPropertyChange(nameof(RestaurantCount));
            }
        }

        public async Task GetRestaurantsAsync()
        {
            Restaurants = new ObservableCollection<RestaurantBObj>(await _restaurantFetchDataManager.GetRestaurantsAsync());
        }

        public async Task GetTotalRestaurantCountAsync()
        {
            RestaurantCount = await _restaurantFetchDataManager.GetTotalRestaurantCountAsync();
        }

        public void GetRestaurant(string sortingKey)
        {
            GetRestaurantRequestObj requestObj = new GetRestaurantRequestObj();

            if(sortingKey.Equals("Relevance"))
            {
                requestObj.Filter = RestaurantFilter.Relevance;
            }
            else if(sortingKey.Equals("Delivery Time"))
            {
                requestObj.Filter = RestaurantFilter.DeliveryTime;
            }
            else if(sortingKey.Equals("Rating"))
            {
                requestObj.Filter = RestaurantFilter.Rating;
            }
            else if (sortingKey.Equals("Cost : Low To High"))
            {
                requestObj.Filter = RestaurantFilter.CostLowToHigh;
            }
            else if (sortingKey.Equals("Cost : High To Low"))
            {
                requestObj.Filter = RestaurantFilter.CostHighToLow;
            }

            GetRestaurant usecase = new GetRestaurant(requestObj,new GetRestaurantPresenterCallback(this));
            usecase.Execute();
        }

    }
}
