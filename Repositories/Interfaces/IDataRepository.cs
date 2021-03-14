using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IDataRepository
    {
        Task<List<TableModel>> GetTablesAsync(string dbConnectionString);

        DataSet GetRow(string tableName, string columnName, string columnType, object id);

        Task UpdateAsync(dynamic model);

        Task DeleteRowAsync(string tableName, string columnName, string columnType, object value);

        Task<dynamic> CreateAsync(dynamic model);

        DataSet GetTableData(string tableName);
    }
}
