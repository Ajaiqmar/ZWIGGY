using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Adapter.Database;
using Zwiggy.Core.DataHandler.Database.Contract;
using Zwiggy.Core.DataManager.Contract;
using Zwiggy.Core.Model;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.Usecases.RemoveFromCart;

namespace Zwiggy.Core.DataManager
{
    class RemoveFromCartDataManager : IRemoveFromCartDataManager
    {
        private ISQLAdapter _adapter;
        private IDBHandler _dbHandler;

        public RemoveFromCartDataManager(ISQLAdapter adapter, IDBHandler dbHandler)
        {
            _adapter = adapter;
            _dbHandler = dbHandler;
        }

        public void RemoveFromCart(DishBObj dish,User user, IUsecaseCallback<RemoveFromCartResponseObj> callback)
        {
            try
            {
                _dbHandler.RemoveDishFromCart(_adapter, dish, user);
                callback.OnSuccess(new RemoveFromCartResponseObj() { DishObj = dish });
            }
            catch (Exception ex)
            {
                callback.OnError(ex);
            }
        }
    }
}
