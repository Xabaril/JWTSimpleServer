using JWTSimpleServer.EntityFrameworkCoreRefreshTokenStore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleServerEFCoreStorage
{
    public class JwtSimpleServerContextFactory : IDesignTimeDbContextFactory<JwtSimpleServerDbContext>
    {
        public JwtSimpleServerDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Startup>()
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<JwtSimpleServerDbContext>();
            optionsBuilder.UseSqlServer(
                configuration["ConnectionStrings:DefaultConnection"],
                sqlServerOptions => sqlServerOptions.MigrationsAssembly(typeof(Startup).Assembly.FullName));

            return new JwtSimpleServerDbContext(optionsBuilder.Options);
        }
    }
}
