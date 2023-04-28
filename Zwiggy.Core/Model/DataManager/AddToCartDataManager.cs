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
using Zwiggy.Core.Notifications;
using Zwiggy.Core.Usecases.AddToCart;
using Zwiggy.Core.Usecases.Contract;

namespace Zwiggy.Core.DataManager
{
    internal class AddToCartDataManager : IAddToCartDataManager
    {
        private ISQLAdapter _adapter;
        private IDBHandler _dbHandler;

        public AddToCartDataManager(ISQLAdapter adapter, IDBHandler dbHandler)
        {
            _adapter = adapter;
            _dbHandler = dbHandler;
        }

        public void AddDishToCart(DishBObj dish, User user, IUsecaseCallback<AddToCartResponseObj> callback)
        {
            try
            {
                _dbHandler.AddDishToCart(_adapter, dish, user);
                callback.OnSuccess(new AddToCartResponseObj() { DishObj = dish });
                ZwiggyNotification.NotifyCartUpdatedEvent(dish, true);
            }
            catch (Exception ex)
            {
                callback.OnError(ex);
            }
        }
    }
}
