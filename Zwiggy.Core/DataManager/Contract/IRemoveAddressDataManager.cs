using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Model;
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.Usecases.RemoveAddress;

namespace Zwiggy.Core.DataManager.Contract
{
    interface IRemoveAddressDataManager
    {
        void RemoveAddress(Address address, IUsecaseCallback<RemoveAddressResponseObj> callback);
    }
}
