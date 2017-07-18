using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ctorx.Core.Data
{
    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        TDbContext DbContext;
        bool IsDisposed;

        /// <summary>
        /// ctor the Mighty
        /// </summary>
        public UnitOfWork(TDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        /// <summary>
        /// Commits the changes
        /// </summary>
        public void Commit()
        {
            this.DbContext.SaveChanges();
        }

        /// <summary>
        /// Commits the changes 
        /// </summary>
        public async Task CommitAsync()
        {
            await this.DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Rollsback the Changes
        /// </summary>
        public void Rollback()
        {
            // see: http://stackoverflow.com/questions/16437083/dbcontext-discard-changes-without-disposing
            foreach (var entry in this.DbContext.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
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
                    this.DbContext = null;
                }

                this.IsDisposed = true;
            }
        }
    }
}