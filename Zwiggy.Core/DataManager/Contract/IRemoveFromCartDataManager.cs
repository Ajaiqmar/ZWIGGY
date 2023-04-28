using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Model;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.Usecases.RemoveFromCart;

namespace Zwiggy.Core.DataManager.Contract
{
    interface IRemoveFromCartDataManager
    {
        void RemoveFromCart(DishBObj dish,User user,IUsecaseCallback<RemoveFromCartResponseObj> callback);
    }
}
