using Microsoft.EntityFrameworkCore;

namespace Repositories.Data
{
    public class DataContext : DbContext
    {
        public readonly string _connectionString;
        
        public DataContext(DbContextOptions<DataContext> options, string connectionString)
            : base(options)
        {
            _connectionString = connectionString;
        }
    }
}
