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

namespace Zwiggy.Core.Usecases.AddToCart
{
    public class AddToCart : UsecaseBase<AddToCartResponseObj>
    {
        private AddToCartRequestObj _requestObj;
        private IAddToCartDataManager _dataManager;

        public AddToCart(IPresenterCallback<AddToCartResponseObj> presenterCallback, AddToCartRequestObj requestObj) : base(presenterCallback)
        {
            _dataManager = DependencyInjector.GetInstance<IAddToCartDataManager>();
            _requestObj = requestObj;
        }

        public override void Action()
        {
            _dataManager.AddDishToCart(_requestObj.DishObj, _requestObj.UserObj, new AddToCartCallback(_presenterCallback));
        }

        private class AddToCartCallback : IUsecaseCallback<AddToCartResponseObj>
        {
            private IPresenterCallback<AddToCartResponseObj> _presenterCallback;

            public AddToCartCallback(IPresenterCallback<AddToCartResponseObj> presenterCallback)
            {
                _presenterCallback = presenterCallback;
            }

            public void OnSuccess(AddToCartResponseObj responseObj)
            {
                _presenterCallback?.OnSuccess(responseObj);
            }

            public void OnError(Exception ex)
            {
                _presenterCallback?.OnError(ex);
            }
        }
    }

    public class AddToCartResponseObj
    {
        public DishBObj DishObj;
    }

    public class AddToCartRequestObj
    {
        public DishBObj DishObj { get; set; }
        public User UserObj { get; set; }
    }
}
