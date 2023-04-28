using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.Usecases.GetRestaurantSearchResults;

namespace Zwiggy.Core.DataManager.Contract
{
    interface ISearchRestaurantDataManager
    {
        Task SearchRestaurants(string searchQuery,IUsecaseCallback<GetRestaurantSearchResultsResponseObj> callback);
    }
}
