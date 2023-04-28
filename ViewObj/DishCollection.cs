using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zwiggy.ViewObj
{
    class DishCollection : List<Object>
    {
        private bool _showDish = true;

        public DishCollection(IEnumerable<object> items) : base(items)
        { }

        public string Key { get; set; }
        public bool ShowDish
        {
            get
            {
                return _showDish;
            }

            set
            {
                _showDish = value;
            }
        }
        public string ListViewHeaderArrow
        {
            get
            {
                if (_showDish)
                {
                    return "\xE014";
                }

                return "\xE015";
            }
        }
        public override string ToString()
        {
            return Key;
        }

    }
}
