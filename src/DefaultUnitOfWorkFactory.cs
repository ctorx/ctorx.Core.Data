using Microsoft.EntityFrameworkCore;

namespace ctorx.Core.Data
{
    public class DefaultUnitOfWorkFactory<TDbContext> : IUnitOfWorkFactory<TDbContext> where TDbContext : DbContext
    {
        readonly TDbContext DbContext;

        /// <summary>
        /// ctor the Mighty
        /// </summary>
        public DefaultUnitOfWorkFactory(TDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        /// <summary>
        /// Makes a new unit of work
        /// </summary>
        public IUnitOfWork<TDbContext> NewUnitOfWork()
        {
            return new UnitOfWork<TDbContext>(this.DbContext);
        }
    }
}