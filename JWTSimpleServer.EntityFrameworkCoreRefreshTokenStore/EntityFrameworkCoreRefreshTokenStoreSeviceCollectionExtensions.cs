using JWTSimpleServer.Abstractions;
using JWTSimpleServer.EntityFrameworkCoreRefreshTokenStore;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add EF database support to JwtSimpleServer.
    /// </summary>
    public static class EntityFrameworkCoreRefreshTokenStoreSeviceCollectionExtensions
    {
        /// <summary>
        /// Configures EF implementation of RefreshTokenStore with JwtSimpleServer.
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="jwtStoreOptions">The store options action.</param>
        /// <returns></returns>
        public static IServiceCollection AddJwtEntityFrameworkCoreRefreshTokenStore(this IServiceCollection services, Action<JwtStoreOptions> jwtStoreOptions)
        {
            var options = new JwtStoreOptions();
            jwtStoreOptions(options);

            services.AddDbContext<JwtSimpleServerDbContext>(builder =>
            {
                options.ConfigureDbContext?.Invoke(builder);
            });

            services.AddScoped<IRefreshTokenStore, EntityFrameworkCoreRefreshTokenStore>();
            return services;
        }
            
    }
}
