using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ctorx.Core.Data
{
    public abstract class DbContextRepository<TDbContext> : IDbContextRepository<TDbContext> where TDbContext : DbContext
    {
        readonly IDbContextResolver DbContextResolver;

        protected DbContextRepository(IDbContextResolver dbContextResolver )
        {
            this.DbContextResolver = dbContextResolver;
        }

        /// <summary>
        /// Gets the Context
        /// </summary>
        public DbContext Context
        {
            get { return this.DbContextResolver.GetContext(); }
        }

        /// <summary>
        /// Gets a set of the specified type
        /// </summary>
        public IQueryable<TEntity> GetSet<TEntity>() where TEntity : class
        {
            return this.Context.Set<TEntity>();
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

            this.Context.Set<TEntity>().Add(entity);
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

            this.Context.Set<TEntity>().Attach(entity);
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

            this.Context.Set<TEntity>().Remove(entity);
        }        
    }
}