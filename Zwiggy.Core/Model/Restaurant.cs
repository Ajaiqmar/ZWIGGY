using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zwiggy.Core.Model
{
    public class Restaurant
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public long CostForTwo { get; set; }
        public string ImagePath { get; set; }
    }
}
