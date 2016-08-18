using Microsoft.EntityFrameworkCore;

namespace ctorx.Core.Data
{
    public abstract class DbContextService<TDbContext> : IDbContextService where TDbContext : DbContext
    {
        protected readonly IDbContextRepository<TDbContext> Repository;

        /// <summary>
        /// ctor the Mighty
        /// </summary>
        protected DbContextService(IDbContextRepository<TDbContext> repository)
        {
            this.Repository = repository;
        }

        /// <summary>
        /// Adds an Entity
        /// </summary>
        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            this.Repository.Add(entity);
        }

        /// <summary>
        /// Attaches an Entity
        /// </summary>
        public void Attach<TEntity>(TEntity entity) where TEntity : class
        {
            this.Repository.Attach(entity);
        }

        /// <summary>
        /// Deletes the Entity
        /// </summary>
        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            this.Repository.Delete(entity);
        }
    }
}