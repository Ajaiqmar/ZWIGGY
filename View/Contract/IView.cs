using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Zwiggy.View.Contract
{
    interface IView
    {
        CoreDispatcher GetDispatcher();
    }
}
