using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.DataManager.Contract;
using Zwiggy.Core.DependencyService;
using Zwiggy.Core.Model;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Usecases.Contract;

namespace Zwiggy.Core.Usecases.GetFavouriteRestaurants
{
    public class GetFavouriteRestaurants : UsecaseBase<GetFavouriteRestaurantsResponseObj>
    {
        private GetFavouriteRestaurantsRequestObj _requestObj;
        private IFetchFavouriteRestaurantsDataManager _dataManager;

        public GetFavouriteRestaurants(GetFavouriteRestaurantsRequestObj requestObj, IPresenterCallback<GetFavouriteRestaurantsResponseObj> presenterCallback) : base(presenterCallback)
        {
            _requestObj = requestObj;
            _dataManager = DependencyInjector.GetInstance<IFetchFavouriteRestaurantsDataManager>();
        }

        public override void Action()
        {
            _dataManager.GetFavouriteRestaurants(_requestObj.UserObj, new GetFavouriteRestaurantsCallback(_presenterCallback));
        }

        private class GetFavouriteRestaurantsCallback : IUsecaseCallback<GetFavouriteRestaurantsResponseObj>
        {
            private IPresenterCallback<GetFavouriteRestaurantsResponseObj> _presenterCallback;

            public GetFavouriteRestaurantsCallback(IPresenterCallback<GetFavouriteRestaurantsResponseObj> presenterCallback)
            {
                _presenterCallback = presenterCallback;
            }

            public void OnError(Exception ex)
            {
                _presenterCallback.OnError(ex);
            }

            public void OnSuccess(GetFavouriteRestaurantsResponseObj responseObj)
            {
                _presenterCallback.OnSuccess(responseObj);
            }
        }
    }

    public class GetFavouriteRestaurantsResponseObj
    {
        public IList<RestaurantBObj> Restaurants;
    }

    public class GetFavouriteRestaurantsRequestObj
    {
        public User UserObj;
    }
}
