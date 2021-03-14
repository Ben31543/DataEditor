using Models;
using Repositories.Data;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
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

        public async Task DeleteRowAsync(string tableName, string columnName, string columnType, object value)
        {
            Type typeOfColumn = Type.GetType(columnType);
            var columnValue = Convert.ChangeType(value, typeOfColumn) ?? null;

            if ((columnValue is null) is false)
            {
                string queryString = $"DELETE * " +
                    $"FROM {tableName} " +
                    $"WHERE {columnName} = {columnValue}";

                using (SqlConnection connection = new SqlConnection(_context._connectionString))
                {
                    connection.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);
                    DataSet dataSet = new DataSet();
                    //DataRow 
                }
            }
        }

        public DataSet GetTableData(string tableName)
        {
            string queryString = $"SELECT * FROM {tableName}";

            using (SqlConnection connection = new SqlConnection(_context._connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }

        public DataSet GetRow(string tableName, string columnName, string columnType, object id)
        {
            Type type = Type.GetType(columnType);
            var value = Convert.ChangeType(id, type);

            if ((value is null) is false)
            {
                string queryString = $"SELECT * " +
                    $"FROM {tableName} " +
                    $"WHERE {columnName} = {value}";

                using (SqlConnection connection = new SqlConnection(_context._connectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    return ds;
                }
            }
            return null;
        }

        public async Task UpdateAsync(dynamic model)
        {

        }

        public async Task<List<TableModel>> GetTablesAsync(string dbConnectionString)
        {
            List<TableModel> tables = new List<TableModel>();
            string queryString = "SELECT name FROM sys.tables";

            using (SqlConnection connection = new SqlConnection(dbConnectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        tables.Add(new TableModel
                        {
                            Name = reader.GetValue(0) as string
                        });
                    }
                    reader.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
            return tables.ToList();
        }
    }
}
