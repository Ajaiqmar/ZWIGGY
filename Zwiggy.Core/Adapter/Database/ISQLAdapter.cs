using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zwiggy.Core.Adapter.Database
{
    public interface ISQLAdapter
    {
        void ExecuteQuery(string query, ArrayList parameters);
        void ExecuteQuery(string query);
        Task<IList<Object>> ExecuteReaderAsync(string query, ArrayList parameters);
        Task<IList<Object>> ExecuteReaderAsync(string query);
    }
}
