using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.Adapter.Database;
using Zwiggy.Core.DataHandler.Database.Contract;
using Zwiggy.Core.Model;
using Zwiggy.Core.ModelBObj;

namespace Zwiggy.Core.DataHandler.Database
{
    public class DBHandler : IDBHandler
    {
        private ISQLAdapter _adapter = new SQLAdapter();

        public void AddUser(User user)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(user.Email);
            parameters.Add(user.Name);
            parameters.Add(user.Password);

            _adapter.ExecuteQuery("INSERT INTO users VALUES($1,$2,$3);", parameters);
        }

        public async Task<bool> UserExists(string email)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(email);

            IList<Object> results = await _adapter.ExecuteReaderAsync("SELECT email FROM users where email=$1;", parameters);

            return !(results.Count == 0);
        }

        public async Task<bool> ValidateUserCredentials(User user)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(user.Email);
            parameters.Add(user.Password);

            IList<Object> results = await _adapter.ExecuteReaderAsync("SELECT * FROM users where email=$1 AND password=$2;", parameters);

            return !(results.Count == 0);
        }

        public async Task<IList<RestaurantBObj>> GetRestaurantsAsync()
        {
            IList<Object> results = await _adapter.ExecuteReaderAsync("SELECT * FROM restaurants;");

            IList<RestaurantBObj> restaurants = new List<RestaurantBObj>();

            foreach (ArrayList result in results)
            {
                RestaurantBObj restaurant = new RestaurantBObj { Id = (string)result[0], Name = (string)result[1], Latitude = (string)result[2], Longitude = (string)result[3], CostForTwo = (long)result[4], ImagePath = (string)result[5] };
                restaurants.Add(restaurant);
            }

            for (int i = 0; i < restaurants.Count; i++)
            {
                ArrayList parameters = new ArrayList();

                parameters.Add(restaurants[i].Id);

                results = await _adapter.ExecuteReaderAsync("SELECT * FROM cuisines,cuisineCategories WHERE cuisines.restaurantId=$1 AND cuisines.cuisineCategoryId=cuisineCategories.id;", parameters);

                IList<Cuisine> cuisines = new List<Cuisine>();

                foreach (ArrayList result in results)
                {
                    cuisines.Add(new Cuisine
                    {
                        Id = (string)result[2],
                        Category = (string)result[3]
                    });
                }

                restaurants[i].Cuisines = cuisines;
            }

            return restaurants;
        }

        public async Task<IList<RestaurantBObj>> GetRestaurants(RestaurantFilter filter,ISQLAdapter adapter)
        {
            IList<Object> results;

            if(filter == RestaurantFilter.Relevance)
            {
                results = await adapter.ExecuteReaderAsync("SELECT * FROM restaurants;");
            }
            else if(filter == RestaurantFilter.DeliveryTime)
            {
                results = await adapter.ExecuteReaderAsync("SELECT * FROM restaurants;");
            }
            else if(filter == RestaurantFilter.Rating)
            {
                results = await adapter.ExecuteReaderAsync("SELECT * FROM restaurants;");
            }
            else if(filter == RestaurantFilter.CostHighToLow)
            {
                results = await adapter.ExecuteReaderAsync("SELECT * FROM restaurants ORDER BY costForTwo DESC;");
            }
            else
            {
                results = await adapter.ExecuteReaderAsync("SELECT * FROM restaurants ORDER BY costForTwo;");
            }

            IList<RestaurantBObj> restaurants = new List<RestaurantBObj>();

            foreach (ArrayList result in results)
            {
                RestaurantBObj restaurant = new RestaurantBObj { Id = (string)result[0], Name = (string)result[1], Latitude = (string)result[2], Longitude = (string)result[3], CostForTwo = (long)result[4], ImagePath = (string)result[5] };
                restaurants.Add(restaurant);
            }

            for (int i = 0; i < restaurants.Count; i++)
            {
                ArrayList parameters = new ArrayList();

                parameters.Add(restaurants[i].Id);

                results = await _adapter.ExecuteReaderAsync("SELECT * FROM cuisines,cuisineCategories WHERE cuisines.restaurantId=$1 AND cuisines.cuisineCategoryId=cuisineCategories.id;", parameters);

                IList<Cuisine> cuisines = new List<Cuisine>();

                foreach (ArrayList result in results)
                {
                    cuisines.Add(new Cuisine
                    {
                        Id = (string)result[2],
                        Category = (string)result[3]
                    });
                }

                restaurants[i].Cuisines = cuisines;
            }

            return restaurants;
        }


        public async Task<long> GetTotalRestaurantCountAsync()
        {
            IList<Object> results = await _adapter.ExecuteReaderAsync("SELECT COUNT(*) FROM restaurants;");

            return (long)(results[0] as ArrayList)[0];
        }

        public void AddFavouriteRestaurant(User user, RestaurantBObj restaurant)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(user.Email);
            parameters.Add(restaurant.Id);

            _adapter.ExecuteQuery("INSERT INTO favouriteRestaurants VALUES($1,$2);", parameters);
        }

        public void RemoveFavouriteRestaurant(User user, RestaurantBObj restaurant)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(user.Email);
            parameters.Add(restaurant.Id);

            _adapter.ExecuteQuery("DELETE FROM favouriteRestaurants WHERE userId=$1 AND restaurantId=$2;", parameters);
        }

        public async Task IsFavouriteRestaurantsAsync(User user, RestaurantBObj restaurant)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(user.Email);
            parameters.Add(restaurant.Id);

            IList<Object> results = await _adapter.ExecuteReaderAsync("SELECT * FROM favouriteRestaurants WHERE userId=$1 AND restaurantId=$2;", parameters);

            restaurant.IsFavourite = (results.Count == 1);
        }

        public async Task<IDictionary<string, IList<DishBObj>>> GetDishesAsync(RestaurantBObj restaurant,User user)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(restaurant.Id);
            parameters.Add(user.Email);

            IList<Object> results = await _adapter.ExecuteReaderAsync("SELECT * FROM (SELECT * FROM dishes,dishCategories WHERE dishes.restaurantId=$1 AND dishes.dishCategoryId=dishCategories.id) dishes LEFT JOIN cart ON dishes.id=cart.dishId AND cart.userId=$2;", parameters);

            IDictionary<string, IList<DishBObj>> dishes = new Dictionary<string, IList<DishBObj>>();

            foreach (ArrayList result in results)
            {
                DishBObj dish = new DishBObj
                {
                    Id = (string)result[0],
                    Name = (string)result[1],
                    Cost = (double)result[2],
                    Description = (string)result[3],
                    IsVeg = (((long)result[4]) != 0),
                    SoldCount = (long)result[5],
                    ImagePath = (string)result[6],
                    RestaurantId = (string)result[8],
                    Category = new DishCategory
                    {
                        Id = (string)result[7],
                        Category = (string)result[10]
                    },
                    DishCount = ((result[12] is long) ? (long)result[12] : 0)
                };

                string dishCategory = dish.Category.Category;

                if (dishes.ContainsKey(dishCategory))
                {
                    dishes[dishCategory].Add(dish);
                }
                else
                {
                    dishes[dishCategory] = new List<DishBObj>();
                    dishes[dishCategory].Add(dish);
                }
            }

            return dishes;
        }

        public async Task<IDictionary<string, IList<DishBObj>>> GetDishesAsync(RestaurantBObj restaurant, IDictionary<string, bool> dishCategoriesVisibility,User user)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(restaurant.Id);
            parameters.Add(user.Email);

            IList<Object> results = await _adapter.ExecuteReaderAsync("SELECT * FROM (SELECT * FROM dishes,dishCategories WHERE dishes.restaurantId=$1 AND dishes.dishCategoryId=dishCategories.id) dishes LEFT JOIN cart ON dishes.id=cart.dishId AND cart.userId=$2;", parameters);

            IDictionary<string, IList<DishBObj>> dishes = new Dictionary<string, IList<DishBObj>>();

            foreach (ArrayList result in results)
            {
                DishBObj dish = new DishBObj
                {
                    Id = (string)result[0],
                    Name = (string)result[1],
                    Cost = (double)result[2],
                    Description = (string)result[3],
                    IsVeg = (((long)result[4]) != 0),
                    SoldCount = (long)result[5],
                    ImagePath = (string)result[6],
                    RestaurantId = (string)result[8],
                    Category = new DishCategory
                    {
                        Id = (string)result[7],
                        Category = (string)result[10]
                    },
                    DishCount = ((result[12] is long) ? (long)result[12] : 0)
                };

                string category = dish.Category.Category;

                if (!dishCategoriesVisibility.ContainsKey(category))
                {
                    continue;
                }
                else if (!dishCategoriesVisibility[category])
                {
                    dishes[category] = new List<DishBObj>();
                }
                else
                {
                    if (dishes.ContainsKey(category))
                    {
                        dishes[category].Add(dish);
                    }
                    else
                    {
                        dishes[category] = new List<DishBObj>();
                        dishes[category].Add(dish);
                    }
                }
            }

            return dishes;
        }

        public async Task<IDictionary<string, IList<DishBObj>>> GetDishesAsync(RestaurantBObj restaurant, bool isVeg,User user)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(restaurant.Id);
            parameters.Add(user.Email);

            IList<Object> results = null;

            if (isVeg)
            {
                parameters.Add(1);
                results = await _adapter.ExecuteReaderAsync("SELECT * FROM (SELECT * FROM dishes,dishCategories WHERE dishes.restaurantId=$1 AND dishes.dishCategoryId=dishCategories.id AND dishes.isVeg=$3) dishes LEFT JOIN cart ON dishes.id=cart.dishId AND cart.userId=$2;", parameters);
            }
            else
            {
                results = await _adapter.ExecuteReaderAsync("SELECT * FROM (SELECT * FROM dishes,dishCategories WHERE dishes.restaurantId=$1 AND dishes.dishCategoryId=dishCategories.id) dishes LEFT JOIN cart ON dishes.id=cart.dishId AND cart.userId=$2;", parameters);
            }


            IDictionary<string, IList<DishBObj>> dishes = new Dictionary<string, IList<DishBObj>>();

            foreach (ArrayList result in results)
            {
                DishBObj dish = new DishBObj
                {
                    Id = (string)result[0],
                    Name = (string)result[1],
                    Cost = (double)result[2],
                    Description = (string)result[3],
                    IsVeg = (((long)result[4]) != 0),
                    SoldCount = (long)result[5],
                    ImagePath = (string)result[6],
                    RestaurantId = (string)result[8],
                    Category = new DishCategory
                    {
                        Id = (string)result[7],
                        Category = (string)result[10]
                    },
                    DishCount = ((result[12] is long) ? (long)result[12] : 0)
                };

                string category = dish.Category.Category;

                if (dishes.ContainsKey(category))
                {
                    dishes[category].Add(dish);
                }
                else
                {
                    dishes[category] = new List<DishBObj>();
                    dishes[category].Add(dish);
                }
            }

            return dishes;
        }

        public async Task<long> GetCartDishesCountAsync(User user)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(user.Email);

            IList<Object> results = await _adapter.ExecuteReaderAsync("SELECT count(*) FROM cart WHERE userId=$1;", parameters);

            return (long)(results[0] as ArrayList)[0];
        }

        public void AddDishToCart(ISQLAdapter adapter, DishBObj dish, User user)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(dish.Id);
            parameters.Add(dish.DishCount);
            parameters.Add(user.Email);

            adapter.ExecuteQuery("INSERT INTO cart VALUES($1,$2,$3);", parameters);
        }

        public void RemoveDishFromCart(DishBObj dish, User user)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(dish.Id);
            parameters.Add(user.Email);

            _adapter.ExecuteQuery("DELETE FROM cart WHERE dishId=$1 AND userId=$2;", parameters);
        }

        public void RemoveDishFromCart(ISQLAdapter adapter,DishBObj dish, User user)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(dish.Id);
            parameters.Add(user.Email);

            adapter.ExecuteQuery("DELETE FROM cart WHERE dishId=$1 AND userId=$2;", parameters);
        }

        public async Task<CartBObj> GetCartAsync(ISQLAdapter adapter, User user)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(user.Email);

            IList<Object> results = await adapter.ExecuteReaderAsync("SELECT * FROM cart,dishes,restaurants WHERE cart.userId=$1 AND cart.dishId=dishes.Id AND restaurants.Id=dishes.restaurantId;", parameters).ConfigureAwait(false);

            bool isRestaurantPickedSet = false;
            CartBObj cart = new CartBObj();
            cart.Dishes = new ObservableCollection<DishBObj>();
            cart.BillAmount = 0.0;
            cart.RestaurantPicked = null;

            foreach (ArrayList result in results)
            {
                cart.Dishes.Add(new DishBObj()
                {
                    Id = (string)result[3],
                    Name = (string)result[4],
                    Cost = (double)result[5],
                    Description = (string)result[6],
                    IsVeg = ((((long)result[7]) == 1) ? true : false),
                    ImagePath = (string)result[9],
                    RestaurantId = (string)result[11],
                    DishCount = (long)result[1]
                });

                cart.BillAmount += (cart.Dishes[cart.Dishes.Count - 1].Cost * cart.Dishes[cart.Dishes.Count - 1].DishCount);

                if (!isRestaurantPickedSet)
                {
                    cart.RestaurantPicked = new Restaurant()
                    {
                        Id = (string)result[12],
                        Name = (string)result[13],
                        ImagePath = (string)result[17]
                    };

                    isRestaurantPickedSet = true;
                }

            }

            return cart;
        }

        public void UpdateDishCountInCart(DishBObj dish, User user)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(dish.DishCount);
            parameters.Add(dish.Id);
            parameters.Add(user.Email);

            _adapter.ExecuteQuery("UPDATE cart SET dishCount=$1 WHERE dishId=$2 AND userId=$3;", parameters);
        }

        public void UpdateDishInCart(ISQLAdapter adapter,DishBObj dish, User user)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(dish.DishCount);
            parameters.Add(dish.Id);
            parameters.Add(user.Email);

            adapter.ExecuteQuery("UPDATE cart SET dishCount=$1 WHERE dishId=$2 AND userId=$3;", parameters);
        }

        public void ClearCart(User user)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(user.Email);

            _adapter.ExecuteQuery("DELETE FROM cart WHERE userId=$1;", parameters);
        }

        public void AddAddress(Address address,ISQLAdapter adapter)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(address.Id);
            parameters.Add(address.AddressDescription);
            parameters.Add(address.Nickname);
            parameters.Add(address.UserId);

            adapter.ExecuteQuery("INSERT INTO address VALUES($1,$2,$3,$4);", parameters);
        }

        public async Task<IList<Address>> GetAddressAsync(User user,ISQLAdapter adapter)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(user.Email);

            IList<Object> results = await adapter.ExecuteReaderAsync("SELECT * FROM address WHERE userId=$1;", parameters);

            IList<Address> addresses = new List<Address>();

            foreach (ArrayList result in results)
            {
                addresses.Add(new Address
                {
                    Id = (string)result[0],
                    AddressDescription = (string)result[1],
                    Nickname = (string)result[2],
                    UserId = (string)result[3]
                });
            }

            return addresses;
        }

        public void AddOrder(OrderBObj order,ISQLAdapter adapter)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(order.Id);
            parameters.Add(order.RestaurantPicked.Id);
            parameters.Add(order.AddressPicked.Id);
            parameters.Add((long)order.ModeOfPayment);
            parameters.Add(order.UserId);
            parameters.Add(order.OrderPlacedDateAndTime.ToString("yyyy-MM-dd HH:mm:ss"));
            parameters.Add((order.IsDelivered)?1:0);

            adapter.ExecuteQuery("INSERT INTO orders VALUES($1,$2,$3,$4,$5,$6,$7);",parameters);

            foreach(DishBObj dish in order.Dishes)
            {
                parameters = new ArrayList();

                parameters.Add(order.Id);
                parameters.Add(dish.Id);
                parameters.Add(dish.DishCount);
                parameters.Add(order.UserId);

                adapter.ExecuteQuery("INSERT INTO orderItems VALUES($1,$2,$3,$4);", parameters);
            }
        }

        public async Task<IList<RestaurantBObj>> GetFavouriteRestaurants(User user,ISQLAdapter adapter)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(user.Email);

            IList<Object> results = await adapter.ExecuteReaderAsync("SELECT * FROM restaurants, favouriteRestaurants WHERE restaurants.id = favouriteRestaurants.restaurantId AND favouriteRestaurants.userId = $1;",parameters);

            IList<RestaurantBObj> restaurants = new List<RestaurantBObj>();

            foreach (ArrayList result in results)
            {
                RestaurantBObj restaurant = new RestaurantBObj { Id = (string)result[0], Name = (string)result[1], Latitude = (string)result[2], Longitude = (string)result[3], CostForTwo = (long)result[4], ImagePath = (string)result[5] };
                restaurants.Add(restaurant);
            }

            for (int i = 0; i < restaurants.Count; i++)
            {
                parameters = new ArrayList();

                parameters.Add(restaurants[i].Id);

                results = await adapter.ExecuteReaderAsync("SELECT * FROM cuisines,cuisineCategories WHERE cuisines.restaurantId=$1 AND cuisines.cuisineCategoryId=cuisineCategories.id;", parameters);

                IList<Cuisine> cuisines = new List<Cuisine>();

                foreach (ArrayList result in results)
                {
                    cuisines.Add(new Cuisine
                    {
                        Id = (string)result[2],
                        Category = (string)result[3]
                    });
                }

                restaurants[i].Cuisines = cuisines;
            }

            return restaurants;
        }

        public void RemoveAddress(Address address, ISQLAdapter adapter)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(address.Id);
            parameters.Add(address.UserId);

            adapter.ExecuteQuery("DELETE FROM address WHERE id=$1 AND userId=$2;", parameters);
        }

        public async Task<IList<OrderBObj>> GetOrders(User user, ISQLAdapter adapter)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(user.Email);

            IList<Object> results = await adapter.ExecuteReaderAsync("SELECT * FROM orders,restaurants WHERE orders.userId=$1 AND orders.restaurantId=restaurants.id ORDER BY orders.orderPlacedDateAndTime DESC;", parameters);

            IList<OrderBObj> orders = new List<OrderBObj>();

            foreach(ArrayList result in results)
            {
                orders.Add(new OrderBObj()
                {
                    Id = (string)result[0],
                    RestaurantPicked = new Restaurant()
                    {
                        Id = (string)result[1],
                        Name = (string)result[8],
                        ImagePath = (string)result[12]
                    },
                    OrderPlacedDateAndTime = DateTime.Parse((string)result[5])
                });
            }

            return orders;
        }

        public async Task<OrderBObj> GetOrderDetails(OrderBObj order, ISQLAdapter adapter)
        {
            OrderBObj orderDetails = new OrderBObj();
            ArrayList parameters = new ArrayList();
            IList<Object> results;

            parameters.Add(order.Id);

            results = await adapter.ExecuteReaderAsync("SELECT * FROM orders,restaurants WHERE orders.id = $1 AND orders.restaurantId=restaurants.Id;", parameters);

            orderDetails.RestaurantPicked = new Restaurant()
            {
                Id = (string)(results[0] as ArrayList)[7],
                Name = (string)(results[0] as ArrayList)[8],
                ImagePath = (string)(results[0] as ArrayList)[12]
            };

            results = await adapter.ExecuteReaderAsync("SELECT * FROM orders,address WHERE orders.id = $1 AND orders.addressId=address.id;", parameters);

            orderDetails.OrderPlacedDateAndTime = order.OrderPlacedDateAndTime;
            orderDetails.AddressPicked = new Address()
            {
                Id = (string)(results[0] as ArrayList)[7],
                AddressDescription = (string)(results[0] as ArrayList)[8],
                Nickname = (string)(results[0] as ArrayList)[9]
            };
            orderDetails.ModeOfPayment = (Payment)Convert.ToInt32((results[0] as ArrayList)[3]);
            orderDetails.IsDelivered = ((((long)(results[0] as ArrayList)[6])==0)?false:true);
            orderDetails.Dishes = new ObservableCollection<DishBObj>();

            results = await adapter.ExecuteReaderAsync("SELECT * FROM orders,orderItems,dishes WHERE orders.id=$1 AND orders.id=orderItems.orderId AND orderItems.dishId=dishes.id;", parameters);

            foreach(ArrayList result in results)
            {
                DishBObj dish = new DishBObj()
                {
                    Id = (string)result[11],
                    Name = (string)result[12],
                    Cost = (double)result[13],
                    IsVeg = ((((long)result[15]) == 1) ? true : false),
                    ImagePath = (string)result[17],
                    DishCount = (long)result[9]
                };

                orderDetails.Dishes.Add(dish);
                orderDetails.BillAmount += dish.TotalDishCost;
            }

            return orderDetails;
        }

        public void UpdateOrderDeliveryStatus(User user,ISQLAdapter adapter)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add(user.Email);

            adapter.ExecuteQuery("UPDATE orders SET isDelivered=1 WHERE userId=$1;", parameters);
        }

        public async Task<IList<RestaurantBObj>> SearchRestaurants(string searchQuery, ISQLAdapter adapter)
        {
            ArrayList parameters = new ArrayList();

            parameters.Add("%"+searchQuery+"%");

            IList<Object> results = await adapter.ExecuteReaderAsync("SELECT * FROM restaurants WHERE restaurants.name LIKE $1;", parameters);

            IList<RestaurantBObj> restaurants = new List<RestaurantBObj>();

            foreach (ArrayList result in results)
            {
                RestaurantBObj restaurant = new RestaurantBObj { Id = (string)result[0], Name = (string)result[1], Latitude = (string)result[2], Longitude = (string)result[3], CostForTwo = (long)result[4], ImagePath = (string)result[5] };
                restaurants.Add(restaurant);
            }

            for (int i = 0; i < restaurants.Count; i++)
            {
                parameters = new ArrayList();

                parameters.Add(restaurants[i].Id);

                results = await adapter.ExecuteReaderAsync("SELECT * FROM cuisines,cuisineCategories WHERE cuisines.restaurantId=$1 AND cuisines.cuisineCategoryId=cuisineCategories.id;", parameters);

                IList<Cuisine> cuisines = new List<Cuisine>();

                foreach (ArrayList result in results)
                {
                    cuisines.Add(new Cuisine
                    {
                        Id = (string)result[2],
                        Category = (string)result[3]
                    });
                }

                restaurants[i].Cuisines = cuisines;
            }

            return restaurants;
        }
    }
}
