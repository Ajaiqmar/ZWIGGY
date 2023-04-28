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

namespace Zwiggy.Core.Usecases.GetCart
{
    public class GetCart : UsecaseBase<GetCartResponseObj>
    {
        private GetCartRequestObj _requestObj;
        private IGetCartDataManager _dataManager;

        public GetCart(IPresenterCallback<GetCartResponseObj> presenterCallback, GetCartRequestObj requestObj) : base(presenterCallback)
        {
            _requestObj = requestObj;
            _dataManager = DependencyInjector.GetInstance<IGetCartDataManager>();
        }

        public override void Action()
        {
            _dataManager.GetCart(_requestObj.UserObj, new GetCartCallback(_presenterCallback));
        }

        private class GetCartCallback : IUsecaseCallback<GetCartResponseObj>
        {
            private IPresenterCallback<GetCartResponseObj> _presenterCallback;

            public GetCartCallback(IPresenterCallback<GetCartResponseObj> presenterCallback)
            {
                _presenterCallback = presenterCallback;
            }

            public void OnSuccess(GetCartResponseObj responseObj)
            {
                _presenterCallback.OnSuccess(responseObj);
            }

            public void OnError(Exception ex)
            {
                _presenterCallback.OnError(ex);
            }
        }
    }

    public class GetCartResponseObj
    {
        public CartBObj Cart;
    }

    public class GetCartRequestObj
    {
        public User UserObj;
    }
}
