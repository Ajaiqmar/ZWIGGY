using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Model;

namespace Zwiggy.Core.ModelBObj
{
    public class DishBObj : Dish
    {
        private long _dishCount;

        public DishCategory Category { get; set; }
        public long DishCount
        {
            get
            {
                return _dishCount;
            }
            set
            {
                _dishCount = value;
                OnPropertyChange(nameof(DishCount));
                OnPropertyChange(nameof(TotalDishCost));
            }
        }
        public double TotalDishCost
        {
            get
            {
                return DishCount * Cost;
            }
        }
    }
}
