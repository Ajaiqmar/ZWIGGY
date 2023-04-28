using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Adapter.Database;
using Zwiggy.Core.DataHandler.Database.Contract;
using Zwiggy.Core.DataManager.Contract;
using Zwiggy.Core.Model;
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.Usecases.RemoveAddress;

namespace Zwiggy.Core.DataManager
{
    class RemoveAddressDataManager : IRemoveAddressDataManager
    {
        private ISQLAdapter _adapter;
        private IDBHandler _dbHandler;

        public RemoveAddressDataManager(ISQLAdapter adapter, IDBHandler dbHandler)
        {
            _adapter = adapter;
            _dbHandler = dbHandler;
        }

        public void RemoveAddress(Address address,IUsecaseCallback<RemoveAddressResponseObj> callback)
        {
            try
            {
                _dbHandler.RemoveAddress(address, _adapter);
                callback.OnSuccess(new RemoveAddressResponseObj()
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
