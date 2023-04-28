using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Model;
using Zwiggy.Core.Usecases.AddAddress;
using Zwiggy.Core.Usecases.Contract;

namespace Zwiggy.Core.DataManager.Contract
{
    interface IAddAddressDataManager
    {
        void AddAddress(Address address,IUsecaseCallback<AddAddressResponseObj> callback);
    }
}
