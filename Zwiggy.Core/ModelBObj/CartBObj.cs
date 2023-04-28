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
    public class CartBObj : BindingsNotification
    {
        private double _billAmount;
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
            }
        }
    }
}
