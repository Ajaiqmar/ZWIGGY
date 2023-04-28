using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.DataManager.Contract;
using Zwiggy.Core.DependencyService;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Usecases.Contract;

namespace Zwiggy.Core.Usecases.GetOrderDetails
{
    public class GetOrderDetails : UsecaseBase<GetOrderDetailsResponseObj>
    {
        private GetOrderDetailsRequestObj _requestObj;
        private IGetOrderDetailsDataManager _dataManager;

        public GetOrderDetails(IPresenterCallback<GetOrderDetailsResponseObj> presenterCallback, GetOrderDetailsRequestObj requestObj) : base(presenterCallback)
        {
            _requestObj = requestObj;
            _dataManager = DependencyInjector.GetInstance<IGetOrderDetailsDataManager>();
        }

        public override void Action()
        {
            _dataManager.GetOrderDetails(_requestObj.OrderObj, new GetOrderDetailsCallback(_presenterCallback));
        }

        private class GetOrderDetailsCallback : IUsecaseCallback<GetOrderDetailsResponseObj>
        {
            private IPresenterCallback<GetOrderDetailsResponseObj> _presenterCallback;

            public GetOrderDetailsCallback(IPresenterCallback<GetOrderDetailsResponseObj> presenterCallback)
            {
                _presenterCallback = presenterCallback;
            }

            public void OnSuccess(GetOrderDetailsResponseObj responseObj)
            {
                _presenterCallback.OnSuccess(responseObj);
            }

            public void OnError(Exception ex)
            {
                _presenterCallback.OnError(ex);
            }
        }
    }

    public class GetOrderDetailsResponseObj
    {
        public OrderBObj OrderObj;
    }

    public class GetOrderDetailsRequestObj
    {
        public OrderBObj OrderObj;
    }
}
