using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Model;
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.Usecases.GetFavouriteRestaurants;

namespace Zwiggy.Core.DataManager.Contract
{
    interface IFetchFavouriteRestaurantsDataManager
    {
        Task GetFavouriteRestaurants(User user, IUsecaseCallback<GetFavouriteRestaurantsResponseObj> callback);
    }
}
