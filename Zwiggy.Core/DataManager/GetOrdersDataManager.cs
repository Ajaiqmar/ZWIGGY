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
using Zwiggy.Core.Usecases.GetOrders;

namespace Zwiggy.Core.DataManager
{
    internal class GetOrdersDataManager : IGetOrdersDataManager
    {
        private ISQLAdapter _adapter;
        private IDBHandler _dbHandler;

        public GetOrdersDataManager(ISQLAdapter adapter, IDBHandler dbHandler)
        {
            _adapter = adapter;
            _dbHandler = dbHandler;
        }

        public async Task GetOrders(User user,IUsecaseCallback<GetOrdersResponseObj> callback)
        {
            try
            {
                IList<OrderBObj> orders = await _dbHandler.GetOrders(user,_adapter);

                callback.OnSuccess(new GetOrdersResponseObj
                {
                    Orders = orders
                });
            }
            catch(Exception ex)
            {
                callback.OnError(ex);
            }
        }
    }
}
