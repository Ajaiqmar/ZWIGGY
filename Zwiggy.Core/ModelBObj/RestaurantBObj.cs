using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Model;

namespace Zwiggy.Core.ModelBObj
{
    public class RestaurantBObj : Restaurant
    {
        public bool IsFavourite { get; set; }
        public IList<Cuisine> Cuisines { get; set; }
        public DateTime DeliveryTime { get; set; }
    }
}
