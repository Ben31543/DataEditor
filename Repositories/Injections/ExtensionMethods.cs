using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repositories.Data;
using Repositories.Interfaces;
using Repositories.Repositories;

namespace Repositories.Injections
{
    public static class ExtensionMethods
    {
        public static void AddRepositoryInjections(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton(new DataContext(new DbContextOptions<DataContext>(), connectionString));
            services.AddTransient<IDataRepository, DataRepository>();
        }
    }
}
