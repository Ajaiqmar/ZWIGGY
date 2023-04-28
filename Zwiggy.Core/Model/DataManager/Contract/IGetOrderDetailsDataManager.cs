using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.Usecases.GetOrderDetails;

namespace Zwiggy.Core.DataManager.Contract
{
    interface IGetOrderDetailsDataManager
    {
        Task GetOrderDetails(OrderBObj order, IUsecaseCallback<GetOrderDetailsResponseObj> callback);
    }
}
