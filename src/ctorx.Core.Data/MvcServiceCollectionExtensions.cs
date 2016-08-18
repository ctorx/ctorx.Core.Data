using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ctorx.Core.Data
{
    public static class MvcServiceCollectionExtensions
    {
        /// <summary>
        /// Wires up the UoW framework for a specific DbContext
        /// </summary>
        public static IServiceCollection AddUoW<TDbContext, TConcreteRepository>(this IServiceCollection serviceCollection) 
            where TDbContext : DbContext 
            where TConcreteRepository : DbContextRepository<TDbContext>
        {
            serviceCollection.AddScoped<IDbContextResolver<TDbContext>, DefaultDbContextResolver<TDbContext>>();
            serviceCollection.AddScoped<IUnitOfWorkFactory<TDbContext>, DefaultUnitOfWorkFactory<TDbContext>>();
            serviceCollection.AddScoped<IDbContextRepository<TDbContext>, TConcreteRepository>();

            return serviceCollection;
        }
    }
}