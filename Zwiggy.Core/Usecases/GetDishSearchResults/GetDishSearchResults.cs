using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Usecases.Contract;

namespace Zwiggy.Core.Usecases.GetDishSearchResults
{
    public class GetDishSearchResults : UsecaseBase<GetDishSearchResultsResponseObj>
    {
        private GetDishSearchResultsRequestObj _requestObj;
        //private IFetchRestaurantDataManager _dataManager;

        public GetDishSearchResults(GetDishSearchResultsRequestObj requestObj, IPresenterCallback<GetDishSearchResultsResponseObj> presenterCallback) : base(presenterCallback)
        {
            _requestObj = requestObj;
            //_dataManager = DependencyInjector.GetInstance<IFetchRestaurantDataManager>();
        }

        public override void Action()
        {
            //_dataManager.GetRestaurants(_requestObj.Filter, new GetRestaurantSearchResultsCallback(_presenterCallback));
        }

        private class GetRestaurantSearchResultsCallback : IUsecaseCallback<GetDishSearchResultsResponseObj>
        {
            private IPresenterCallback<GetDishSearchResultsResponseObj> _presenterCallback;

            public GetRestaurantSearchResultsCallback(IPresenterCallback<GetDishSearchResultsResponseObj> presenterCallback)
            {
                _presenterCallback = presenterCallback;
            }

            public void OnError(Exception ex)
            {
                _presenterCallback.OnError(ex);
            }

            public void OnSuccess(GetDishSearchResultsResponseObj responseObj)
            {
                _presenterCallback.OnSuccess(responseObj);
            }
        }
    }

    public class GetDishSearchResultsRequestObj
    {
        public string SearchQuery;
    }

    public class GetDishSearchResultsResponseObj
    {
        public IList<DishBObj> Dishes;
    }
}
