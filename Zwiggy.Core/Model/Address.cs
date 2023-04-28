using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Utility;

namespace Zwiggy.Core.Model
{
    public class Address : BindingsNotification
    {
        private string _addressDescription;
        private string _nickname;

        public string Id { get; set; }
        public string UserId { get; set; }


        public string AddressDescription 
        { 
            get
            {
                return _addressDescription;
            }

            set
            {
                _addressDescription = value;
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
    }
}
