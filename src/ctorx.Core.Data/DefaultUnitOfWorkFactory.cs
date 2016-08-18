using Microsoft.EntityFrameworkCore;

namespace ctorx.Core.Data
{
    public class DefaultUnitOfWorkFactory<TDbContext> : IUnitOfWorkFactory<TDbContext> where TDbContext : DbContext
    {
        readonly IDbContextResolver<TDbContext> DataContextResolver;

        /// <summary>
        /// ctor the Mighty
        /// </summary>
        public DefaultUnitOfWorkFactory(IDbContextResolver<TDbContext> dataContextResolver)
        {
            this.DataContextResolver = dataContextResolver;
        }

        /// <summary>
        /// Makes a new unit of work
        /// </summary>
        public IUnitOfWork<TDbContext> NewUnitOfWork()
        {
            return new UnitOfWork<TDbContext>(this.DataContextResolver);
        }
    }
}