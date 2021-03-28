using Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IDataRepository
    {
        Task<List<TableModel>> GetTablesAsync(string dbConnectionString);

        DataSet GetRow(string tableName, string columnName, object id);

        Task UpdateAsync(string tableName, string pkColumnName, object id, Dictionary<string, string> tableValues);

        Task DeleteRowAsync(string tableName, string columnName, string columnType, object value);

        Task<DataSet> CreateAsync(Dictionary<string, string> values, string tableName);

        DataSet GetTableData(string tableName);
    }
}
