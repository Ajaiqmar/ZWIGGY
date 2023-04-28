using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Model;
using Zwiggy.Core.Usecases.Contract;
using Zwiggy.Core.Usecases.GetCart;

namespace Zwiggy.Core.DataManager.Contract
{
    internal interface IGetCartDataManager
    {
        Task GetCart(User user, IUsecaseCallback<GetCartResponseObj> callback);
    }
}
