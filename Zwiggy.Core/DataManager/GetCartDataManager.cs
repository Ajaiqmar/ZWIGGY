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
using Zwiggy.Core.Usecases.GetCart;

namespace Zwiggy.Core.DataManager
{
    internal class GetCartDataManager : IGetCartDataManager
    {
        private ISQLAdapter __adapter;
        private IDBHandler _dbHandler;

        public GetCartDataManager(ISQLAdapter _adapter, IDBHandler dbHandler)
        {
            __adapter = _adapter;
            _dbHandler = dbHandler;
        }

        public async Task GetCart(User user, IUsecaseCallback<GetCartResponseObj> callback)
        {
            try
            {
                callback.OnSuccess(new GetCartResponseObj() { Cart = await _dbHandler.GetCartAsync(__adapter, user).ConfigureAwait(false) });
            }
            catch (Exception ex)
            {
                callback.OnError(ex);
            }
        }
    }
}
