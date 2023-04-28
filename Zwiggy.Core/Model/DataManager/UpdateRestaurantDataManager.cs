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
    public class UpdateRestaurantDataManager
    {
        private DBHandler _dbHandler = new DBHandler();

        public void AddFavouriteRestaurant(User user, RestaurantBObj restaurant)
        {
            _dbHandler.AddFavouriteRestaurant(user, restaurant);
        }

        public void RemoveFavouriteRestaurant(User user, RestaurantBObj restaurant)
        {
            _dbHandler.RemoveFavouriteRestaurant(user, restaurant);
        }
    }
}
