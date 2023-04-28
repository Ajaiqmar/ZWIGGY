using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Model;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Usecases.AddToCart;
using Zwiggy.Core.Usecases.Contract;

namespace Zwiggy.Core.DataManager.Contract
{
    internal interface IAddToCartDataManager
    {
        void AddDishToCart(DishBObj dish, User user, IUsecaseCallback<AddToCartResponseObj> callback);
    }
}
