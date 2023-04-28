using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.DependencyService;
using Zwiggy.Core.Model;
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.DataManager.Contract;

namespace Zwiggy.Core.Usecases.RemoveAddress
{
    public class RemoveAddress : UsecaseBase<RemoveAddressResponseObj>
    {
        private RemoveAddressRequestObj _requestObj;
        private IRemoveAddressDataManager _dataManager;

        public RemoveAddress(RemoveAddressRequestObj requestObj, IPresenterCallback<RemoveAddressResponseObj> presenterCallback) : base(presenterCallback)
        {
            _requestObj = requestObj;
            _dataManager = DependencyInjector.GetInstance<IRemoveAddressDataManager>();
        }

        public override void Action()
        {
            _dataManager.RemoveAddress(_requestObj.AddressObj, new RemoveAddressCallback(_presenterCallback));
        }

        private class RemoveAddressCallback : IUsecaseCallback<RemoveAddressResponseObj>
        {
            private IPresenterCallback<RemoveAddressResponseObj> _presenterCallback;

            public RemoveAddressCallback(IPresenterCallback<RemoveAddressResponseObj> presenterCallback)
            {
                _presenterCallback = presenterCallback;
            }

            public void OnError(Exception ex)
            {
                _presenterCallback.OnError(ex);
            }

            public void OnSuccess(RemoveAddressResponseObj responseObj)
            {
                _presenterCallback.OnSuccess(responseObj);
            }
        }
    }

    public class RemoveAddressResponseObj
    {
        public Address AddressObj;
    }

    public class RemoveAddressRequestObj
    {
        public Address AddressObj;
    }
}
