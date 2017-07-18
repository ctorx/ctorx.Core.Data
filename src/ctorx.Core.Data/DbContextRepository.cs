using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ctorx.Core.Data
{
    public abstract class DbContextRepository<TDbContext> : IDbContextRepository<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// Gets the DbContext
        /// </summary>
        public TDbContext DbContext { get; }

        protected DbContextRepository(TDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        /// <summary>
        /// Gets a set of the specified type
        /// </summary>
        public IQueryable<TEntity> GetSet<TEntity>() where TEntity : class
        {
            return this.DbContext.Set<TEntity>();
        }

        /// <summary>
        /// Adds an Entity
        /// </summary>
        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            this.DbContext.Set<TEntity>().Add(entity);
        }

        /// <summary>
        /// Attaches an Entity
        /// </summary>
        public void Attach<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            this.DbContext.Set<TEntity>().Attach(entity);
        }

        /// <summary>
        /// Deletes the Entity
        /// </summary>
        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            this.DbContext.Set<TEntity>().Remove(entity);
        }
    }
}