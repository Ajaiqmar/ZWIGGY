using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zwiggy.Core.Usecases.Contract
{
    public abstract class UsecaseBase<T> : IUsecaseBase<T>
    {
        protected IPresenterCallback<T> _presenterCallback;

        public UsecaseBase(IPresenterCallback<T> presenterCallback)
        {
            _presenterCallback = presenterCallback;
        }

        public abstract void Action();

        public void Execute()
        {
            if (GetIfAvailableInCache())
            {
                return;
            }

            Task.Run(() => {

                try
                {
                    Action();
                }
                catch (Exception ex)
                {
                    _presenterCallback?.OnError(ex);
                }

            });
        }

        public bool GetIfAvailableInCache()
        {
            return false;
        }
    }
}
