using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.DataHandler.Database;
using Zwiggy.Core.Model;

namespace Zwiggy.Core.DataManager
{
    public class FetchCartDataManager
    {
        private DBHandler _dbHandler = new DBHandler();

        //public async Task<CartBObj> GetCartAsync(User user)
        //{
        //    return await _dbHandler.GetCartAsync(user);
        //}

        public async Task<long> GetCartDishesCountAsync(User user)
        {
            return await _dbHandler.GetCartDishesCountAsync(user);
        }
    }
}
