using System;
using Microsoft.EntityFrameworkCore;

namespace ctorx.Core.Data
{
    public interface IDbContextResolver : IDisposable
    {
        /// <summary>
        /// Resolves the Data Context
        /// </summary>
        DbContext GetContext();

        /// <summary>
        /// Destroys the Context
        /// </summary>
        void DestroyContext();
    }

    public interface IDbContextResolver<TDbContext> : IDbContextResolver where TDbContext : DbContext { }
}