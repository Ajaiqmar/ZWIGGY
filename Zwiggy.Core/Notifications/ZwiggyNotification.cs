using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.ZwiggyEventArgs;

namespace Zwiggy.Core.Notifications
{
    public class ZwiggyNotification
    {
        public static event Action<UpdateCartEventArgs> CartUpdated;

        internal static void NotifyCartUpdatedEvent(DishBObj dish, bool dishAdded)
        {
            CartUpdated?.Invoke(new UpdateCartEventArgs(dish, dishAdded));
        }
    }
}
