using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Utility;

namespace Zwiggy.Core.Model
{
    public class Order : BindingsNotification
    {
        private Payment _modeOfPayment;
        private bool _isDelivered;

        public string Id { get; set; }
        public string AddressId { get; set; }
        public Payment ModeOfPayment
        {
            get
            {
                return _modeOfPayment;
            }
            set
            {
                _modeOfPayment = value;
                OnPropertyChange(nameof(ModeOfPayment));
            }
        }
        public string UserId { get; set; }
        public DateTime OrderPlacedDateAndTime { get; set; }
        public bool IsDelivered
        {
            get
            {
                return _isDelivered;
            }
            set
            {
                _isDelivered = value;
                OnPropertyChange(nameof(IsDelivered));
            }
        }
    }
}
