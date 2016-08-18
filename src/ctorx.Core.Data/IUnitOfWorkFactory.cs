using Microsoft.EntityFrameworkCore;

namespace ctorx.Core.Data
{
    public interface IUnitOfWorkFactory<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// Makes a unit of work
        /// </summary>
        IUnitOfWork<TDbContext> NewUnitOfWork();
    }
}