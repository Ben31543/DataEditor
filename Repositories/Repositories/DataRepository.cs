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

        public Task DeleteAsync(dynamic model)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DataModel>> GetTableViewAsync(string tableName)
        {
            var data = new List<DataModel>();
            string queryString = $"SELECT * FROM {tableName}";

            using (SqlConnection connection = new SqlConnection(_context._connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                var values = new List<object>();

                foreach (DataTable table in ds.Tables)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            var cells = row.ItemArray;

                            foreach (object cell in cells)
                            {
                                values.Add(cell);
                            }
                            data.Add(new DataModel
                            {
                                Name = column.ColumnName,
                                ColumnType = column.DataType,
                                IsUnique = column.Unique,
                                Values = values
                            });
                        }
                    }
                }
            }
            return data.ToList();
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
