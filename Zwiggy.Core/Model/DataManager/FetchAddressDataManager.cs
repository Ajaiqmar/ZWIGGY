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
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.Usecases.GetAddress;

namespace Zwiggy.Core.DataManager
{
    internal class FetchAddressDataManager : IFetchAddressDataManager
    {
        private ISQLAdapter _adapter;
        private IDBHandler _dbHandler;

        public FetchAddressDataManager(ISQLAdapter adapter, IDBHandler dbHandler)
        {
            _adapter = adapter;
            _dbHandler = dbHandler;
        }

        public async Task GetAddressAsync(User user,IUsecaseCallback<GetAddressResponseObj> callback)
        {
            try
            {
                IList<Address> addresses = await _dbHandler.GetAddressAsync(user, _adapter);
                callback.OnSuccess(new GetAddressResponseObj()
                {
                    Addresses = addresses
                });
            }
            catch(Exception ex)
            {
                callback.OnError(ex);
            }
        }
    }
}
