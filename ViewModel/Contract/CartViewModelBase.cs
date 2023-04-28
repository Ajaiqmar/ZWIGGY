using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Model;
using Zwiggy.Core.ModelBObj;
using Zwiggy.Core.Utility;

namespace Zwiggy.ViewModel.Contract
{
    abstract class CartViewModelBase : BindingsNotification
    {
        private string _address = "";
        private string _nickname = "";
        private ObservableCollection<Address> _addresses = null;
        private bool _isAddressSectionEmpty = true;
        private bool _isCartEmpty = true;
        private CartBObj _cart = null;

        public string AddressDescription
        {
            get
            {
                return _address;
            }

            set
            {
                _address = value;
                OnPropertyChange(nameof(AddressDescription));
            }
        }

        public string Nickname
        {
            get
            {
                return _nickname;
            }

            set
            {
                _nickname = value;
                OnPropertyChange(nameof(Nickname));
            }
        }

        public ObservableCollection<Address> Addresses
        {
            get
            {
                return _addresses;
            }

            set
            {
                _addresses = value;
                OnPropertyChange(nameof(Addresses));
            }
        }

        public bool IsAddressSectionEmpty
        {
            get
            {
                return _isAddressSectionEmpty;
            }

            set
            {
                _isAddressSectionEmpty = value;
                OnPropertyChange(nameof(IsAddressSectionEmpty));
                OnPropertyChange(nameof(IsAddressSectionNotEmpty));
            }
        }

        public bool IsAddressSectionNotEmpty
        {
            get
            {
                return !_isAddressSectionEmpty;
            }
        }
        
        public bool IsCartEmpty
        {
            get
            {
                return _isCartEmpty;
            }

            set
            {
                _isCartEmpty = value;
                OnPropertyChange(nameof(IsCartEmpty));
                OnPropertyChange(nameof(IsCartNotEmpty));
            }
        }

        public bool IsCartNotEmpty
        {
            get
            {
                return !_isCartEmpty;
            }
        }

        public CartBObj CartObj
        {
            get
            {
                return _cart;
            }

            set
            {
                _cart = value;
                OnPropertyChange(nameof(CartObj));
                OnPropertyChange(nameof(TotalPay));
            }
        }

        public double TotalPay
        {
            get
            {
                if(CartObj != null)
                {
                    return CartObj.BillAmount + 101 + 40;
                }

                return 101 + 40;
            }

            set
            {
                OnPropertyChange(nameof(TotalPay));
            }
        }

        public Address AddressSelected { get; set; }

        public Payment ModeOfPayment { get; set; }

        public abstract void AddAddress();

        public abstract void GetAddress();

        public abstract void GetCart();

        public abstract void AddToCart(DishBObj dish);
        public abstract void RemoveFromCart(DishBObj dish);
        public abstract void PlaceOrder();
    }
}
