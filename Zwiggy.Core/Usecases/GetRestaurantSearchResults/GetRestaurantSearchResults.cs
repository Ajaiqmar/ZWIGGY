using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.DataManager.Contract;
using Zwiggy.Core.DependencyService;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Usecases.Contract;

namespace Zwiggy.Core.Usecases.GetRestaurantSearchResults
{
    public class GetRestaurantSearchResults : UsecaseBase<GetRestaurantSearchResultsResponseObj>
    {
        private GetRestaurantSearchResultsRequestObj _requestObj;
        private ISearchRestaurantDataManager _dataManager;

        public GetRestaurantSearchResults(GetRestaurantSearchResultsRequestObj requestObj, IPresenterCallback<GetRestaurantSearchResultsResponseObj> presenterCallback) : base(presenterCallback)
        {
            _requestObj = requestObj;
            _dataManager = DependencyInjector.GetInstance<ISearchRestaurantDataManager>();
        }

        public override void Action()
        {
            _dataManager.SearchRestaurants(_requestObj.SearchQuery, new GetRestaurantSearchResultsCallback(_presenterCallback));
        }

        private class GetRestaurantSearchResultsCallback : IUsecaseCallback<GetRestaurantSearchResultsResponseObj>
        {
            private IPresenterCallback<GetRestaurantSearchResultsResponseObj> _presenterCallback;

            public GetRestaurantSearchResultsCallback(IPresenterCallback<GetRestaurantSearchResultsResponseObj> presenterCallback)
            {
                _presenterCallback = presenterCallback;
            }

            public void OnError(Exception ex)
            {
                _presenterCallback.OnError(ex);
            }

            public void OnSuccess(GetRestaurantSearchResultsResponseObj responseObj)
            {
                _presenterCallback.OnSuccess(responseObj);
            }
        }
    }

    public class GetRestaurantSearchResultsRequestObj
    {
        public string SearchQuery;
    }

    public class GetRestaurantSearchResultsResponseObj
    {
        public IList<RestaurantBObj> Restaurants;
    }
}
