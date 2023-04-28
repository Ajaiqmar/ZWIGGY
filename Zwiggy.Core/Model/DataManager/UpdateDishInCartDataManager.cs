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
using Zwiggy.Core.Usecases.UpdateDishInCart;

namespace Zwiggy.Core.DataManager
{
    class UpdateDishInCartDataManager : IUpdateDishInCartDataManager
    {
        private ISQLAdapter _adapter;
        private IDBHandler _dbHandler;

        public UpdateDishInCartDataManager(ISQLAdapter adapter, IDBHandler dbHandler)
        {
            _adapter = adapter;
            _dbHandler = dbHandler;
        }

        public void UpdateDishInCart(DishBObj dish,User user, IUsecaseCallback<UpdateDishInCartResponseObj> callback)
        {
            try
            {
                _dbHandler.UpdateDishInCart(_adapter, dish, user);
                callback.OnSuccess(new UpdateDishInCartResponseObj() { DishObj = dish });
            }
            catch (Exception ex)
            {
                callback.OnError(ex);
            }
        }
    }
}
