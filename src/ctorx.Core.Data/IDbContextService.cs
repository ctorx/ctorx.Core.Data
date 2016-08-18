namespace ctorx.Core.Data
{
    public interface IDbContextService
    {
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