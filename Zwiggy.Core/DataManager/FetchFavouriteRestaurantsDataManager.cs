using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Adapter.Database;
using Zwiggy.Core.DataHandler.Database.Contract;
using Zwiggy.Core.DataManager.Contract;
using Zwiggy.Core.Model;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.Usecases.GetFavouriteRestaurants;

namespace Zwiggy.Core.DataManager
{
    class FetchFavouriteRestaurantsDataManager : IFetchFavouriteRestaurantsDataManager
    {
        private IDBHandler _dbHandler;
        private ISQLAdapter _adapter;

        public FetchFavouriteRestaurantsDataManager(ISQLAdapter adapter, IDBHandler dbHandler)
        {
            _dbHandler = dbHandler;
            _adapter = adapter;
        }

        public async Task GetFavouriteRestaurants(User user, IUsecaseCallback<GetFavouriteRestaurantsResponseObj> callback)
        {
            try
            {
                IList<RestaurantBObj> restaurants = await _dbHandler.GetFavouriteRestaurants(user,_adapter);

                callback.OnSuccess(new GetFavouriteRestaurantsResponseObj()
                {
                    Restaurants = restaurants
                });
            }
            catch(Exception ex)
            {
                callback.OnError(ex);
            }
        }
    }
}
