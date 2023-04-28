using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zwiggy.Core.Usecases.Contract
{
    public interface IUsecaseCallback<T>
    {
        void OnSuccess(T responseObj);
        void OnError(Exception ex);
    }
}
