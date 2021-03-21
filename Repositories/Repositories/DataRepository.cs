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
using Repositories.Injections;

namespace Repositories.Repositories
{
    public class DataRepository : IDataRepository
    {
        private readonly DataContext _context;

        public DataRepository(DataContext context)
        {
            _context = context;
        }

        public DataSet Create(DataModel dataModel, string tableName)
        {
            string queryString = $"INSERT INTO {tableName} " +
                                 $"({dataModel.Columns.TurnIntoSqlColumns()}) " +
                                 $"VALUES ({ExtensionMethods.TurnIntoSqlColumnValuePair(dataModel.Columns, dataModel.Values)});";

            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);

                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                dataSet.AcceptChanges();

                return dataSet;
            }
        }

        public async Task DeleteRowAsync(string tableName, string columnName, string columnType, object value)
        {
            Type typeOfColumn = Type.GetType(columnType);
            var columnValue = Convert.ChangeType(value, typeOfColumn) ?? null;

            if ((columnValue is null) is false)
            {
                string queryString = $"DELETE FROM {tableName} WHERE {columnName} = {columnValue}";

                using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
                {
                    connection.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);

                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);

                    dataSet.AcceptChanges();
                }
            }
        }

        public DataSet GetTableData(string tableName)
        {
            string queryString = $"SELECT * FROM {tableName}";

            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }

        public DataSet GetRow(string tableName, string pkColumnName, object id)
        {
            if (id == null)
            {
	            return null;
            }

            string queryString = $"SELECT * " +
                                 $"FROM {tableName} " +
                                 $"WHERE {pkColumnName} = {id}";

            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
	            connection.Open();
	            SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);
	            DataSet ds = new DataSet();
	            adapter.Fill(ds);

	            return ds;
            }
        }

        public async Task UpdateAsync(string tableName, DataModel dataModel)
        {
            string queryString = $"UPDATE {tableName} " +
                                 $"SET {ExtensionMethods.TurnIntoSqlColumnValuePair(dataModel.Columns, dataModel.Values)};";

            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                connection.Open();
                DataSet dataSet = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(queryString, connection);

                adapter.Fill(dataSet);
                dataSet.AcceptChanges();
            }
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
