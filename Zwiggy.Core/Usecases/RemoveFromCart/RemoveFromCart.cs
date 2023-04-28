using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.DataManager.Contract;
using Zwiggy.Core.DependencyService;
using Zwiggy.Core.Model;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Notifications;
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.ZwiggyEventArgs;

namespace Zwiggy.Core.Usecases.RemoveFromCart
{
    public class RemoveFromCart : UsecaseBase<RemoveFromCartResponseObj>
    {
        private RemoveFromCartRequestObj _requestObj;
        private IRemoveFromCartDataManager _dataManager;

        public RemoveFromCart(IPresenterCallback<RemoveFromCartResponseObj> presenterCallback, RemoveFromCartRequestObj requestObj) : base(presenterCallback)
        {
            _requestObj = requestObj;
            _dataManager = DependencyInjector.GetInstance<IRemoveFromCartDataManager>();
        }

        public override void Action()
        {
            _dataManager.RemoveFromCart(_requestObj.DishObj,_requestObj.UserObj, new RemoveFromCartCallback(_presenterCallback));
        }

        private class RemoveFromCartCallback : IUsecaseCallback<RemoveFromCartResponseObj>
        {
            private IPresenterCallback<RemoveFromCartResponseObj> _presenterCallback;

            public RemoveFromCartCallback(IPresenterCallback<RemoveFromCartResponseObj> presenterCallback)
            {
                _presenterCallback = presenterCallback;
            }

            public void OnSuccess(RemoveFromCartResponseObj responseObj)
            {
                _presenterCallback.OnSuccess(responseObj);
                ZwiggyNotification.NotifyCartUpdatedEvent(responseObj.DishObj, false);
            }

            public void OnError(Exception ex)
            {
                _presenterCallback.OnError(ex);
            }
        }
    }

    public class RemoveFromCartResponseObj
    {
        public DishBObj DishObj;
    }

    public class RemoveFromCartRequestObj
    {
        public DishBObj DishObj;
        public User UserObj;
    }
}
