using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.DataManager.Contract;
using Zwiggy.Core.DependencyService;
using Zwiggy.Core.Model;
using Zwiggy.Core.Usecases.Contract;

namespace Zwiggy.Core.Usecases.AddAddress
{
    public class AddAddress : UsecaseBase<AddAddressResponseObj>
    {
        private AddAddressRequestObj _requestObj;
        private IAddAddressDataManager _dataManager;

        public AddAddress(AddAddressRequestObj requestObj,IPresenterCallback<AddAddressResponseObj> presenterCallback) : base(presenterCallback)
        {
            _requestObj = requestObj;
            _dataManager = DependencyInjector.GetInstance<IAddAddressDataManager>();
        }

        public override void Action()
        {
            _dataManager.AddAddress(_requestObj.AddressObj,new AddAddressCallback(_presenterCallback));
        }

        private class AddAddressCallback : IUsecaseCallback<AddAddressResponseObj>
        {
            private IPresenterCallback<AddAddressResponseObj> _presenterCallback;

            public AddAddressCallback(IPresenterCallback<AddAddressResponseObj> presenterCallback)
            {
                _presenterCallback = presenterCallback;
            }

            public void OnError(Exception ex)
            {
                _presenterCallback.OnError(ex);
            }

            public void OnSuccess(AddAddressResponseObj responseObj)
            {
                _presenterCallback.OnSuccess(responseObj);
            }
        }
    }

    public class AddAddressRequestObj
    {
        public Address AddressObj;
    }

    public class AddAddressResponseObj
    {
        public Address AddressObj;
    }
}
