using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.ModelBObj;

namespace Zwiggy.Core.ZwiggyEventArgs
{
    public class UpdateCartEventArgs : EventArgs
    {
        public DishBObj UpdatedDish = null;
        public bool DishAdded;

        public UpdateCartEventArgs(DishBObj dish, bool dishAdded)
        {
            UpdatedDish = dish;
            DishAdded = dishAdded;
        }
    }
}
