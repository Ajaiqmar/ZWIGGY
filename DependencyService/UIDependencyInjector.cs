using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.View;
using Zwiggy.View.Contract;
using Zwiggy.ViewModel;
using Zwiggy.ViewModel.Contract;

namespace Zwiggy.DependencyService
{
    class UIDependencyInjector
    {
        private static IServiceProvider _serviceProvider = null;

        internal static IServiceProvider ServiceProvider
        {
            get
            {
                if(_serviceProvider == null)
                {
                    _serviceProvider = RegisterServices(new ServiceCollection());
                }

                return _serviceProvider;
            }
        }

        internal static T GetInstance<T>()
        {
            return ServiceProvider.GetService<T>();
        }

        internal static IServiceProvider RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<DishViewModelBase, DishViewModel>()
                .AddSingleton<CartViewModelBase, CartViewModel>()
                .AddSingleton<ProfileViewModelBase, ProfileViewModel>()
                .AddSingleton<MainViewModelBase,MainViewModel>();
            return serviceCollection.BuildServiceProvider();
        }
    }
}
