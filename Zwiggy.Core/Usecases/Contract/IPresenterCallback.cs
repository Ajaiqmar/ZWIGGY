using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zwiggy.Core.Usecases.Contract
{
    public interface IPresenterCallback<T>
    {
        Task OnSuccess(T responseObj);
        void OnError(Exception ex);
    }
}
