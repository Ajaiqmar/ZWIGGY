using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zwiggy.Core.Usecases.Contract
{
    public interface IUsecaseBase<T>
    {
        void Action();
        void Execute();
        bool GetIfAvailableInCache();
    }
}
