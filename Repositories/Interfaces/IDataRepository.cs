using Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IDataRepository
    {
        Task<List<TableModel>> GetTablesAsync(string dbConnectionString);

        DataSet GetRow(string tableName, string columnName, string columnType, object id);

        Task UpdateAsync(string tableName, DataModel dataModel);

        Task DeleteRowAsync(string tableName, string columnName, string columnType, object value);

        DataSet Create(DataModel dataModel, string tableName);

        DataSet GetTableData(string tableName);
    }
}
