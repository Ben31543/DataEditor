using Microsoft.EntityFrameworkCore;

namespace Repositories.Data
{
    public class DataContext : DbContext
    {
        public string ConnectionString { get; }
        
        public DataContext(DbContextOptions<DataContext> options, string connectionString)
            : base(options)
        {
            ConnectionString = connectionString;
        }
    }
}
