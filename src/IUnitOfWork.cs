using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ctorx.Core.Data
{
    public interface IUnitOfWork<TDbContext> : IDisposable where TDbContext : DbContext
    {
        /// <summary>
        /// Commits the changes
        /// </summary>
        void Commit();

        /// <summary>
        /// Commits the changes 
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Rollsback the Changes
        /// </summary>
        void Rollback();
    }
}