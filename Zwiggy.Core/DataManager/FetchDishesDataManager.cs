using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.DataHandler.Database;
using Zwiggy.Core.Model;
using Zwiggy.Core.ModelBObj;

namespace Zwiggy.Core.DataManager
{
    public class FetchDishesDataManager
    {
        private DBHandler _dbHandler = new DBHandler();

        public async Task<IDictionary<string, IList<DishBObj>>> GetDishesAsync(RestaurantBObj restaurant,User user)
        {
            return await _dbHandler.GetDishesAsync(restaurant,user);
        }

        public async Task<IDictionary<string, IList<DishBObj>>> GetDishesAsync(RestaurantBObj restaurant, IDictionary<string, bool> dishCategoriesVisibility,User user)
        {
            return await _dbHandler.GetDishesAsync(restaurant, dishCategoriesVisibility,user);
        }

        public async Task<IDictionary<string, IList<DishBObj>>> GetDishesAsync(RestaurantBObj restaurant, bool isVeg, User user)
        {
            return await _dbHandler.GetDishesAsync(restaurant, isVeg,user);
        }
    }
}
