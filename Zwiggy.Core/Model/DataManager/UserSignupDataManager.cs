using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zwiggy.Core.DataHandler.Database;
using Zwiggy.Core.Model;

namespace Zwiggy.Core.DataManager
{
    public class UserSignupDataManager
    {
        private DBHandler _dBHandler = new DBHandler();

        public async Task<bool> UserExists(string email)
        {
            return await _dBHandler.UserExists(email);
        }

        public void AddUser(User user)
        {
            _dBHandler.AddUser(user);
        }
    }
}
