using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.DataManager.Contract;
using Zwiggy.Core.DependencyService;
using Zwiggy.Core.Model;
using Zwiggy.Core.Usecases.Contract;

namespace Zwiggy.Core.Usecases.GetAddress
{
    public class GetAddress : UsecaseBase<GetAddressResponseObj>
    {
        private GetAddressRequestObj _requestObj;
        private IFetchAddressDataManager _dataManager;

        public GetAddress(GetAddressRequestObj requestObj, IPresenterCallback<GetAddressResponseObj> presenterCallback) : base(presenterCallback)
        {
            _requestObj = requestObj;
            _dataManager = DependencyInjector.GetInstance<IFetchAddressDataManager>();
        }

        public override void Action()
        {
            _dataManager.GetAddressAsync(_requestObj.UserObj, new AddAddressCallback(_presenterCallback));
        }

        private class AddAddressCallback : IUsecaseCallback<GetAddressResponseObj>
        {
            private IPresenterCallback<GetAddressResponseObj> _presenterCallback;

            public AddAddressCallback(IPresenterCallback<GetAddressResponseObj> presenterCallback)
            {
                _presenterCallback = presenterCallback;
            }

            public void OnError(Exception ex)
            {
                _presenterCallback.OnError(ex);
            }

            public void OnSuccess(GetAddressResponseObj responseObj)
            {
                _presenterCallback.OnSuccess(responseObj);
            }
        }
    }

    public class GetAddressResponseObj
    {
        public IList<Address> Addresses;
    }

    public class GetAddressRequestObj
    {
        public User UserObj;
    }
}
