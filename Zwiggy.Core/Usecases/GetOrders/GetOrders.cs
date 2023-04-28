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

namespace Zwiggy.Core.Usecases.GetOrders
{
    public class GetOrders : UsecaseBase<GetOrdersResponseObj>
    {
        private GetOrdersRequestObj _requestObj;
        private IGetOrdersDataManager _dataManager;

        public GetOrders(IPresenterCallback<GetOrdersResponseObj> presenterCallback, GetOrdersRequestObj requestObj) : base(presenterCallback)
        {
            _requestObj = requestObj;
            _dataManager = DependencyInjector.GetInstance<IGetOrdersDataManager>();
        }

        public override void Action()
        {
            _dataManager.GetOrders(_requestObj.UserObj, new GetOrdersCallback(_presenterCallback));
        }

        private class GetOrdersCallback : IUsecaseCallback<GetOrdersResponseObj>
        {
            private IPresenterCallback<GetOrdersResponseObj> _presenterCallback;

            public GetOrdersCallback(IPresenterCallback<GetOrdersResponseObj> presenterCallback)
            {
                _presenterCallback = presenterCallback;
            }

            public void OnSuccess(GetOrdersResponseObj responseObj)
            {
                _presenterCallback.OnSuccess(responseObj);
            }

            public void OnError(Exception ex)
            {
                _presenterCallback.OnError(ex);
            }
        }
    }

    public class GetOrdersResponseObj
    {
        public IList<OrderBObj> Orders;
    }

    public class GetOrdersRequestObj
    {
        public User UserObj;
    }
}
