using Microsoft.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Zwiggy.Core.Adapter.Database
{
    public class SQLAdapter : ISQLAdapter
    {
        private readonly string _connectionString = $"Data Source = { Path.Combine(ApplicationData.Current.LocalFolder.Path, "swiggy.db") }";
        private SqliteConnection _connection;

        public SQLAdapter()
        {
            _connection = new SqliteConnection(_connectionString);
            this.OpenConnection();
        }

        ~SQLAdapter()
        {
            this.CloseConnection();
        }

        public void OpenConnection()
        {
            _connection?.Open();
        }

        public void CloseConnection()
        {
            _connection?.Close();
        }

        public void ExecuteQuery(string query, ArrayList parameters)
        {

            using (SqliteCommand command = _connection.CreateCommand())
            {
                command.CommandText = query;

                for (int i = 0; i < parameters.Count; i++)
                {
                    command.Parameters.AddWithValue($"${i + 1}", parameters[i]);
                }

                command.ExecuteNonQuery();
            }
        }

        public void ExecuteQuery(string query)
        {
            using (SqliteCommand command = _connection.CreateCommand())
            {
                command.CommandText = query;
                command.ExecuteNonQuery();
            }
        }

        public async Task<IList<Object>> ExecuteReaderAsync(string query, ArrayList parameters)
        {
            using (SqliteCommand command = _connection.CreateCommand())
            {
                command.CommandText = query;

                for (int i = 0; i < parameters.Count; i++)
                {
                    command.Parameters.AddWithValue($"${i + 1}", parameters[i]);
                }

                SqliteDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                IList<Object> results = new List<Object>();

                while (reader.Read())
                {
                    ArrayList tuple = new ArrayList();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        tuple.Add(reader.GetValue(i));
                    }

                    results.Add(tuple);
                }

                return results;
            }
        }

        public async Task<IList<Object>> ExecuteReaderAsync(string query)
        {
            using (SqliteCommand command = _connection.CreateCommand())
            {
                command.CommandText = query;

                SqliteDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

                IList<Object> results = new List<Object>();

                while (reader.Read())
                {
                    ArrayList tuple = new ArrayList();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        tuple.Add(reader.GetValue(i));
                    }

                    results.Add(tuple);
                }

                return results;
            }
        }
    }
}
