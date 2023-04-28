using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.DataHandler.Database;
using Zwiggy.Core.Model;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Notifications;

namespace Zwiggy.Core.DataManager
{
    public class UpdateCartDataManager
    {
        DBHandler _cartDBHandler = new DBHandler();

        public void ClearCart(User user)
        {
            _cartDBHandler.ClearCart(user);
            // FOR INFO BADGE
            ZwiggyNotification.NotifyCartUpdatedEvent(null, false);
        }

        public void UpdateDishCountInCart(DishBObj dish, User user)
        {
            _cartDBHandler.UpdateDishCountInCart(dish, user);
            ZwiggyNotification.NotifyCartUpdatedEvent(dish, false);
        }

        public void RemoveDishFromCart(DishBObj dish, User user)
        {
            _cartDBHandler.RemoveDishFromCart(dish, user);
            ZwiggyNotification.NotifyCartUpdatedEvent(dish, false);
        }
    }
}
