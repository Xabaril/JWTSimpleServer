using JWTSimpleServer.Abstractions;
using JWTSimpleServer.MessagePackRefreshTokenStore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MessagePackRefreshTokenStoreCollectionExtensions
    {
        public static IServiceCollection AddJwtMessagePackRefreshTokenStore(this IServiceCollection services, Action<JwtStoreOptions> setup)
        {
            var jwtStoreOptions = new JwtStoreOptions();
            setup(jwtStoreOptions);
            services.AddSingleton<IRefreshTokenStore>
                (sp => new MessagePackRefreshTokenStore(jwtStoreOptions));
            return services;
        }
    }
}
