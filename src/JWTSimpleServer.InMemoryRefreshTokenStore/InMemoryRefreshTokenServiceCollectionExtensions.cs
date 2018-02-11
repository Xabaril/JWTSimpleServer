using JWTSimpleServer.Abstractions;
using JWTSimpleServer.InMemoryRefreshTokenStore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InMemoryRefreshTokenServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtInMemoryRefreshTokenStore(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IRefreshTokenStore, InMemoryRefreshTokenStore>();
            return services;
        }
    }
}
