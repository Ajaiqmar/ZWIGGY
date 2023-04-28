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
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.Usecases.PlaceOrder;

namespace Zwiggy.Core.DataManager
{
    class PlaceOrderDataManager : IPlaceOrderDataManager
    {
        private ISQLAdapter _adapter;
        private IDBHandler _dbHandler;

        public PlaceOrderDataManager(ISQLAdapter adapter, IDBHandler dbHandler)
        {
            _adapter = adapter;
            _dbHandler = dbHandler;
        }

        public void PlaceOrder(OrderBObj order,User user, IUsecaseCallback<PlaceOrderResponseObj> callback)
        {
            try
            {
                _dbHandler.ClearCart(user);
                _dbHandler.UpdateOrderDeliveryStatus(user,_adapter);
                _dbHandler.AddOrder(order,_adapter);
                ZwiggyNotification.NotifyCartUpdatedEvent(null,false);
                callback.OnSuccess(new PlaceOrderResponseObj() { OrderObj = order });
            }
            catch (Exception ex)
            {
                callback.OnError(ex);
            }
        }
    }
}
