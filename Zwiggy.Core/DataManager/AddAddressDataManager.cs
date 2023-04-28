using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Adapter.Database;
using Zwiggy.Core.DataHandler.Database;
using Zwiggy.Core.DataHandler.Database.Contract;
using Zwiggy.Core.DataManager.Contract;
using Zwiggy.Core.Model;
using Zwiggy.Core.Usecases.AddAddress;
using Zwiggy.Core.Usecases.Contract;

namespace Zwiggy.Core.DataManager
{
    internal class AddAddressDataManager : IAddAddressDataManager
    {
        private ISQLAdapter _adapter;
        private IDBHandler _dbHandler;

        public AddAddressDataManager(ISQLAdapter adapter,IDBHandler dbHandler)
        {
            _adapter = adapter;
            _dbHandler = dbHandler;
        }

        public void AddAddress(Address address,IUsecaseCallback<AddAddressResponseObj> callback)
        {
            try
            {
                _dbHandler.AddAddress(address, _adapter);
                callback.OnSuccess(new AddAddressResponseObj
                {
                    AddressObj = address
                });
            }
            catch(Exception ex)
            {
                callback.OnError(ex);
            }
        }
    }
}
