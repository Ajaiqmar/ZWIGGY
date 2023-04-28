using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Model;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.Usecases.UpdateDishInCart;

namespace Zwiggy.Core.DataManager.Contract
{
    interface IUpdateDishInCartDataManager
    {
        void UpdateDishInCart(DishBObj dish, User user,IUsecaseCallback<UpdateDishInCartResponseObj> callback);
    }
}
