using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ctorx.Core.Data
{
    public interface IDbContextRepository<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// Gets a set of the specified type
        /// </summary>
        IQueryable<TEntity> GetSet<TEntity>() where TEntity : class;

        /// <summary>
        /// Adds an Entity
        /// </summary>
        void Add<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Attaches an Entity
        /// </summary>
        void Attach<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Deletes the Entity
        /// </summary>
        void Delete<TEntity>(TEntity entity) where TEntity : class;
    }
}