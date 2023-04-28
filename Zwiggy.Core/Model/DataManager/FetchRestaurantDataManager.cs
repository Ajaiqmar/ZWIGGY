using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Adapter.Database;
using Zwiggy.Core.DataHandler.Database;
using Zwiggy.Core.DataHandler.Database.Contract;
using Zwiggy.Core.DataManager.Contract;
using Zwiggy.Core.Model;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.Usecases.GetRestaurant;

namespace Zwiggy.Core.DataManager
{
    public class FetchRestaurantDataManager : IFetchRestaurantDataManager
    {
        private DBHandler _dBHandler = new DBHandler();
        private IDBHandler _cdbHandler;
        private ISQLAdapter _adapter;

        public FetchRestaurantDataManager(ISQLAdapter adapter,IDBHandler dbHandler)
        {
            _cdbHandler = dbHandler;
            _adapter = adapter;
        }

        public FetchRestaurantDataManager()
        {}

        public async Task<IList<RestaurantBObj>> GetRestaurantsAsync()
        {
            return await _dBHandler.GetRestaurantsAsync();
        }

        public async void GetRestaurants(RestaurantFilter filter, IUsecaseCallback<GetRestaurantResponseObj> callback)
        {
            try
            {
                IList<RestaurantBObj> restaurants = await _cdbHandler.GetRestaurants(filter, _adapter);

                callback.OnSuccess(new GetRestaurantResponseObj()
                {
                    Restaurants = restaurants
                });
            }
            catch (Exception ex)
            {
                callback.OnError(ex);
            }
        }

        public async Task<long> GetTotalRestaurantCountAsync()
        {
            return await _dBHandler.GetTotalRestaurantCountAsync();
        }

        public async Task IsFavouriteRestaurantAsync(User user, RestaurantBObj restaurant)
        {
            await _dBHandler.IsFavouriteRestaurantsAsync(user, restaurant);
        }
    }
}
