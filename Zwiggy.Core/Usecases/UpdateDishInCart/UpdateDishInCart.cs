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

namespace Zwiggy.Core.Usecases.UpdateDishInCart
{
    public class UpdateDishInCart : UsecaseBase<UpdateDishInCartResponseObj>
    {
        private UpdateDishInCartRequestObj _requestObj;
        private IUpdateDishInCartDataManager _dataManager;

        public UpdateDishInCart(IPresenterCallback<UpdateDishInCartResponseObj> presenterCallback, UpdateDishInCartRequestObj requestObj) : base(presenterCallback)
        {
            _requestObj = requestObj;
            _dataManager = DependencyInjector.GetInstance<IUpdateDishInCartDataManager>();
        }

        public override void Action()
        {
            _dataManager.UpdateDishInCart(_requestObj.DishObj,_requestObj.UserObj, new UpdateDishInCartCallback(_presenterCallback));
        }

        private class UpdateDishInCartCallback : IUsecaseCallback<UpdateDishInCartResponseObj>
        {
            private IPresenterCallback<UpdateDishInCartResponseObj> _presenterCallback;

            public UpdateDishInCartCallback(IPresenterCallback<UpdateDishInCartResponseObj> presenterCallback)
            {
                _presenterCallback = presenterCallback;
            }

            public void OnSuccess(UpdateDishInCartResponseObj responseObj)
            {
                _presenterCallback.OnSuccess(responseObj);
            }

            public void OnError(Exception ex)
            {
                _presenterCallback.OnError(ex);
            }
        }
    }

    public class UpdateDishInCartResponseObj
    {
        public DishBObj DishObj;
    }

    public class UpdateDishInCartRequestObj
    {
        public DishBObj DishObj;
        public User UserObj;
    }
}
