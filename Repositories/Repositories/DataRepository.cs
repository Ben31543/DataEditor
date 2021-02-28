using Models;
using Repositories.Data;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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

        public async Task<List<string>> GetAllDataAsync(string tableName)
        {
            //var data = new Dictionary<string, List<dynamic>>();
            List<string> items = new List<string>();
            string queryString = $"SELECT Name FROM {tableName}";

            using (SqlConnection connection = new SqlConnection(_context._connectionString))
            {
                #region variant 1
                //    SqlCommand command = new SqlCommand(queryString, connection);
                //    try
                //    {
                //        connection.Open();
                //        SqlDataReader reader = await command.ExecuteReaderAsync();
                //        while (await reader.ReadAsync())
                //        {
                //            for (int i = 0; i < reader.FieldCount; i++)
                //            {
                //                var dataColumn = (string)reader.GetValue(i);
                //                items.Add(dataColumn);
                //            }
                //        }
                //        reader.Close();
                //    }
                //    catch (Exception e)
                //    {
                //        Debug.WriteLine(e.Message);
                //    }
                //}
                //return items;
                #endregion
                connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);

                DataSet ds = new DataSet();
                adapter.Fill(ds);

                foreach (DataTable dt in ds.Tables)
                {
                    Console.WriteLine(dt.TableName); 
                    foreach (DataColumn column in dt.Columns)
                    {
                        Console.Write("\t{0}", column.ColumnName);
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        var cells = row.ItemArray;
                        foreach (object cell in cells)
                        {
                            Console.Write("\t{0}", cell);
                        }
                    }
                }
            }
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
            return tables;
        }
    }
}
