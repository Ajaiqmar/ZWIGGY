using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Adapter.Database;
using Zwiggy.Core.DataHandler.Database;
using Zwiggy.Core.DataHandler.Database.Contract;
using Zwiggy.Core.DataManager;
using Zwiggy.Core.DataManager.Contract;

namespace Zwiggy.Core.DependencyService
{
    internal class DependencyInjector
    {
        private static IServiceProvider _serviceProvider = null;

        public static IServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProvider == null)
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

        public static IServiceProvider RegisterServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ISQLAdapter,SQLAdapter>()
                .AddSingleton<IAddToCartDataManager, AddToCartDataManager>()
                .AddSingleton<IGetCartDataManager, GetCartDataManager>()
                .AddSingleton<IAddAddressDataManager,AddAddressDataManager>()
                .AddSingleton<IFetchAddressDataManager,FetchAddressDataManager>()
                .AddSingleton<IUpdateDishInCartDataManager,UpdateDishInCartDataManager>()
                .AddSingleton<IRemoveFromCartDataManager,RemoveFromCartDataManager>()
                .AddSingleton<IPlaceOrderDataManager,PlaceOrderDataManager>()
                .AddSingleton<IFetchRestaurantDataManager,FetchRestaurantDataManager>()
                .AddSingleton<IFetchFavouriteRestaurantsDataManager,FetchFavouriteRestaurantsDataManager>()
                .AddSingleton<IRemoveAddressDataManager,RemoveAddressDataManager>()
                .AddSingleton<IGetOrdersDataManager,GetOrdersDataManager>()
                .AddSingleton<IGetOrderDetailsDataManager,GetOrderDetailsDataManager>()
                .AddSingleton<ISearchRestaurantDataManager,SearchRestaurantDataManager>()
                .AddSingleton<IDBHandler, DBHandler>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
