using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Model;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.Usecases.PlaceOrder;

namespace Zwiggy.Core.DataManager.Contract
{
    interface IPlaceOrderDataManager
    {
        void PlaceOrder(OrderBObj order,User user, IUsecaseCallback<PlaceOrderResponseObj> callback);
    }
}
