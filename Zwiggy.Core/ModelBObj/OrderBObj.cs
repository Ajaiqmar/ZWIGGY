using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Model;
using Zwiggy.Core.Utility;

namespace Zwiggy.Core.ModelBObj
{
    public class OrderBObj : Order
    {
        private double _billAmount;
        private Address _addressPicked;
        private Restaurant _restaurantPicked;
        private ObservableCollection<DishBObj> _dishes;

        public Restaurant RestaurantPicked
        {
            get
            {
                return _restaurantPicked;
            }
            set
            {
                _restaurantPicked = value;
                OnPropertyChange(nameof(RestaurantPicked));
            }
        }
        
        public Address AddressPicked
        {
            get
            {
                return _addressPicked;
            }
            set
            {
                _addressPicked = value;
                OnPropertyChange(nameof(AddressPicked));
            }
        }

        public ObservableCollection<DishBObj> Dishes
        {
            get
            {
                return _dishes;
            }

            set
            {
                _dishes = value;
                OnPropertyChange(nameof(Dishes));
            }
        }

        public double BillAmount
        {
            get
            {
                return _billAmount;
            }

            set
            {
                _billAmount = value;
                OnPropertyChange(nameof(BillAmount));
                OnPropertyChange(nameof(TotalPay));
            }
        }

        public string PaymentModeToString
        {
            get
            {
                if (ModeOfPayment == Payment.PayOnDelivery)
                {
                    return "Pay on Delivery";
                }
                else if (ModeOfPayment == Payment.UPI)
                {
                    return "UPI";
                }
                else
                {
                    return "Credit & Debit card";
                }
            }
        }

        public double TotalPay
        {
            get
            {
                return BillAmount + 101 + 40;
            }
        }

        public bool MapVisibility
        {
            get
            {
                return !IsDelivered;
            }
        }
    }
}
