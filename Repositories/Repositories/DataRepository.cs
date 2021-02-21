using Models;
using Repositories.Data;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class DataRepository : IDataRepository
    {
        private readonly DataContext _context;

        public DataRepository(DataContext context)
        {
            _context = context;
        }

        public Task<dynamic> CreateAsync(dynamic model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(dynamic model)
        {
            throw new NotImplementedException();
        }

        public async Task<List<object>> GetAllDataAsync(string tableName)
        {
            var data = new List<object>();
            List<object> items = new List<object>();
            string queryString = $"SELECT * FROM {tableName}";

            using (SqlConnection connection = new SqlConnection(_context._connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var dataColumn =  reader.GetValue(i);
                            items.Add(dataColumn);
                            data.Add(items);
                        }
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
            return data;
        }

        public Task<dynamic> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(dynamic model)
        {

        }

        public async Task<List<TableModel>> GetTablesAsync(string dbConnectionString)
        {
            List<TableModel> tables = new List<TableModel>();
            string queryString = "SELECT TABLE_NAME FROM information_schema.tables";

            using (SqlConnection connection = new SqlConnection(dbConnectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var table = new TableModel { Name = reader.GetValue(i) as string };
                            tables.Add(table);
                        }
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
            return tables;
        }
    }
}
