using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ctorx.Core.Data
{
    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        readonly IDbContextResolver<TDbContext> DbContextResolver;

        bool IsDisposed;
        DbContext Context => this.DbContextResolver.GetContext();

        /// <summary>
        /// ctor the Mighty
        /// </summary>
        public UnitOfWork(IDbContextResolver<TDbContext> dbContextResolver)
        {
            this.DbContextResolver = dbContextResolver;

            // Multiple units of work for the same context cannot exist
            //this.DbContextResolver.DestroyContext();
        }

        /// <summary>
        /// Commits the changes
        /// </summary>
        public void Commit()
        {
            this.Context.SaveChanges();
        }

        /// <summary>
        /// Commits the changes 
        /// </summary>
        public async Task CommitAsync()
        {
            await this.Context.SaveChangesAsync();
        }

        /// <summary>
        /// Rollsback the Changes
        /// </summary>
        public void Rollback()
        {
            this.Dispose();
        }

        /// <summary>
        /// Disposes the object
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the object
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    this.DbContextResolver.DestroyContext(); ;
                }

                this.IsDisposed = true;
            }
        }
    }
}