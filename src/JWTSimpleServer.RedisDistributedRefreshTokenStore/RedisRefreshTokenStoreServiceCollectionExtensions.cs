using JWTSimpleServer.Abstractions;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RedisRefreshTokenStoreServiceCollectionExtensions
    {
        const string JwtSimpleServerRedisInstanceName = "JwtSimpleServerInstance";
        public static IServiceCollection AddDistributedRedisRefreshStokenStore(
            this IServiceCollection services,
            Action<RedisCacheOptions> cacheSetup
        )
        {           
            services.AddSingleton<IRefreshTokenStore, RedisDistributedRefreshTokenStore>();            
            services.AddDistributedRedisCache(setup => cacheSetup(setup));            
            return services;
        }        
    }
}
