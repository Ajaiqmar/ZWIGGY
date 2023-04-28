using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Adapter.Database;
using Zwiggy.Core.DataHandler.Database.Contract;
using Zwiggy.Core.DataManager.Contract;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.Usecases.GetRestaurantSearchResults;

namespace Zwiggy.Core.DataManager
{
    class SearchRestaurantDataManager : ISearchRestaurantDataManager
    {
        private ISQLAdapter _adapter;
        private IDBHandler _dbHandler;

        public SearchRestaurantDataManager(ISQLAdapter adapter,IDBHandler dbHandler)
        {
            _adapter = adapter;
            _dbHandler = dbHandler;
        }

        public async Task SearchRestaurants(string searchQuery, IUsecaseCallback<GetRestaurantSearchResultsResponseObj> callback)
        {
            try
            {
                IList<RestaurantBObj> restaurants = await _dbHandler.SearchRestaurants(searchQuery,_adapter);
                callback.OnSuccess(new GetRestaurantSearchResultsResponseObj()
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
