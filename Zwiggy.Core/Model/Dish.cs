using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Utility;

namespace Zwiggy.Core.Model
{
    public class Dish : BindingsNotification
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public string Description { get; set; }
        public bool IsVeg { get; set; }
        public long SoldCount { get; set; }
        public string ImagePath { get; set; }
        public string RestaurantId { get; set; }
    }
}
