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

namespace Zwiggy.Core.Usecases.GetRestaurant
{
    public class GetRestaurant : UsecaseBase<GetRestaurantResponseObj>
    {
        private GetRestaurantRequestObj _requestObj;
        private IFetchRestaurantDataManager _dataManager;

        public GetRestaurant(GetRestaurantRequestObj requestObj, IPresenterCallback<GetRestaurantResponseObj> presenterCallback) : base(presenterCallback)
        {
            _requestObj = requestObj;
            _dataManager = DependencyInjector.GetInstance<IFetchRestaurantDataManager>();
        }

        public override void Action()
        {
            _dataManager.GetRestaurants(_requestObj.Filter,new GetRestaurantCallback(_presenterCallback));
        }

        private class GetRestaurantCallback : IUsecaseCallback<GetRestaurantResponseObj>
        {
            private IPresenterCallback<GetRestaurantResponseObj> _presenterCallback;

            public GetRestaurantCallback(IPresenterCallback<GetRestaurantResponseObj> presenterCallback)
            {
                _presenterCallback = presenterCallback;
            }

            public void OnError(Exception ex)
            {
                _presenterCallback.OnError(ex);
            }

            public void OnSuccess(GetRestaurantResponseObj responseObj)
            {
                _presenterCallback.OnSuccess(responseObj);
            }
        }
    }

    public class GetRestaurantResponseObj
    {
        public IList<RestaurantBObj> Restaurants;
    }

    public class GetRestaurantRequestObj
    {
        public RestaurantFilter Filter;
    }
}
