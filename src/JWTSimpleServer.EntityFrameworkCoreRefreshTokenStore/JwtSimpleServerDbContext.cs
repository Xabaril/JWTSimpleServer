using Microsoft.EntityFrameworkCore;
using System;

namespace JWTSimpleServer.EntityFrameworkCoreRefreshTokenStore
{
    /// <summary>
    /// DbContext for the JwtSimpleServer tokens data.
    /// </summary>
    public class JwtSimpleServerDbContext : DbContext
    {
        public DbSet<JwtToken> Tokens { get; set; }

        public JwtSimpleServerDbContext(DbContextOptions<JwtSimpleServerDbContext> options) : base(options)
        {
            if (options == null)
            {
                throw new ArgumentException(nameof(options));
            }
        }
    }
}
