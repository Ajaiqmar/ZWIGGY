using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Adapter.Database;
using Zwiggy.Core.Model;
using Zwiggy.Core.ModelBObj;

namespace Zwiggy.Core.DataHandler.Database.Contract
{
    public interface IDBHandler
    {
        void AddUser(User user);
        Task<bool> UserExists(string email);
        Task<bool> ValidateUserCredentials(User user);
        Task<IList<RestaurantBObj>> GetRestaurantsAsync();
        Task<IList<RestaurantBObj>> GetRestaurants(RestaurantFilter filter,ISQLAdapter adapter);
        Task<IList<RestaurantBObj>> GetFavouriteRestaurants(User user,ISQLAdapter adapter);
        Task<long> GetTotalRestaurantCountAsync();
        void AddFavouriteRestaurant(User user, RestaurantBObj restaurant);
        void RemoveFavouriteRestaurant(User user, RestaurantBObj restaurant);
        Task IsFavouriteRestaurantsAsync(User user, RestaurantBObj restaurant);
        Task<IDictionary<string, IList<DishBObj>>> GetDishesAsync(RestaurantBObj restaurant,User user);
        Task<IDictionary<string, IList<DishBObj>>> GetDishesAsync(RestaurantBObj restaurant, IDictionary<string, bool> dishCategoriesVisibility,User user);
        Task<IDictionary<string, IList<DishBObj>>> GetDishesAsync(RestaurantBObj restaurant, bool isVeg,User user);
        Task<long> GetCartDishesCountAsync(User user);
        void AddDishToCart(ISQLAdapter adapter, DishBObj dish, User user);
        void RemoveDishFromCart(DishBObj dish, User user);
        void RemoveDishFromCart(ISQLAdapter adapter, DishBObj dish, User user);
        Task<CartBObj> GetCartAsync(ISQLAdapter adapter, User user);
        void UpdateDishCountInCart(DishBObj dish, User user);
        void UpdateDishInCart(ISQLAdapter adapter, DishBObj dish, User user);
        void ClearCart(User user);
        void AddAddress(Address address, ISQLAdapter adapter);
        void RemoveAddress(Address address,ISQLAdapter adapter);
        Task<IList<Address>> GetAddressAsync(User user,ISQLAdapter adapter);
        void AddOrder(OrderBObj order,ISQLAdapter adapter);
        void UpdateOrderDeliveryStatus(User user,ISQLAdapter adapter);
        Task<OrderBObj> GetOrderDetails(OrderBObj order,ISQLAdapter adapter);
        Task<IList<OrderBObj>> GetOrders(User user,ISQLAdapter adapter);
        Task<IList<RestaurantBObj>> SearchRestaurants(string searchQuery,ISQLAdapter adapter);
    }
}
