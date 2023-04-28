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

namespace Zwiggy.Core.Usecases.PlaceOrder
{
    public class PlaceOrder : UsecaseBase<PlaceOrderResponseObj>
    {
        private PlaceOrderRequestObj _requestObj;
        private IPlaceOrderDataManager _dataManager;

        public PlaceOrder(IPresenterCallback<PlaceOrderResponseObj> presenterCallback, PlaceOrderRequestObj requestObj) : base(presenterCallback)
        {
            _requestObj = requestObj;
            _dataManager = DependencyInjector.GetInstance<IPlaceOrderDataManager>();
        }

        public override void Action()
        {
            _dataManager.PlaceOrder(_requestObj.OrderObj,_requestObj.UserObj,new PlaceOrderCallback(_presenterCallback));
        }

        private class PlaceOrderCallback : IUsecaseCallback<PlaceOrderResponseObj>
        {
            private IPresenterCallback<PlaceOrderResponseObj> _presenterCallback;

            public PlaceOrderCallback(IPresenterCallback<PlaceOrderResponseObj> presenterCallback)
            {
                _presenterCallback = presenterCallback;
            }

            public void OnSuccess(PlaceOrderResponseObj responseObj)
            {
                _presenterCallback.OnSuccess(responseObj);
            }

            public void OnError(Exception ex)
            {
                _presenterCallback.OnError(ex);
            }
        }
    }

    public class PlaceOrderRequestObj
    {
        public OrderBObj OrderObj;
        public User UserObj;
    }

    public class PlaceOrderResponseObj
    {
        public OrderBObj OrderObj;
    }
}
