using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IDataRepository
    {
        Task<List<TableModel>> GetTablesAsync(string dbConnectionString);

        Task<List<object>> GetAllDataAsync(string tableName);

        Task<dynamic> GetAsync(int id);

        Task UpdateAsync(dynamic model);

        Task DeleteAsync(dynamic model);

        Task<dynamic> CreateAsync(dynamic model);
    }
}
