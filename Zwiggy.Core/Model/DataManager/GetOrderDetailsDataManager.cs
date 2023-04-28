using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Adapter.Database;
using Zwiggy.Core.DataHandler.Database.Contract;
using Zwiggy.Core.DataManager.Contract;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.Usecases.GetOrderDetails;

namespace Zwiggy.Core.DataManager
{
    class GetOrderDetailsDataManager : IGetOrderDetailsDataManager
    {
        private ISQLAdapter _adapter;
        private IDBHandler _dbHandler;

        public GetOrderDetailsDataManager(ISQLAdapter adapter, IDBHandler dbHandler)
        {
            _adapter = adapter;
            _dbHandler = dbHandler;
        }

        public async Task GetOrderDetails(OrderBObj order,IUsecaseCallback<GetOrderDetailsResponseObj> callback)
        {
            try
            {
                OrderBObj orderDetails = await _dbHandler.GetOrderDetails(order,_adapter);

                callback.OnSuccess(new GetOrderDetailsResponseObj()
                {
                    OrderObj = orderDetails
                });
            }
            catch(Exception ex)
            {
                callback.OnError(ex);
            }
        }
    }
}
