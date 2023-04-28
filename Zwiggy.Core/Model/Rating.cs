using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zwiggy.Core.Model
{
    public class Rating
    {
        public string HotelId { get; set; }
        public string UserId { get; set; }
        public double ScaleValue { get; set; }
    }
}
