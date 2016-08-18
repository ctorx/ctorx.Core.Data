using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ctorx.Core.Data
{
    public class DefaultDbContextResolver<TDbContext> : IDbContextResolver<TDbContext> where TDbContext : DbContext
    {
        private readonly IServiceProvider ServiceProvider;
        TDbContext Context;

        bool IsDisposed;

        /// <summary>
        /// ctor the Mighty
        /// </summary>
        public DefaultDbContextResolver(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// Resolves the Data Context
        /// </summary>
        public DbContext GetContext()
        {
            if (this.Context == null)
            {
                this.Context = this.ServiceProvider.GetService<TDbContext>();
            }

            return this.Context;
        }

        /// <summary>
        /// Destroys the Context
        /// </summary>
        public void DestroyContext()
        {
            if (this.Context == null)
            {
                return;
            }

            this.Context.Dispose();
            this.Context = null;
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
                    this.DestroyContext();
                }

                this.IsDisposed = true;
            }
        }
    }
}