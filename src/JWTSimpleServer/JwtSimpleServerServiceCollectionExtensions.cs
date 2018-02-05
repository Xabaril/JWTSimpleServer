using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class JwtSimpleServerServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtSimpleServer(this IServiceCollection services)
        {
            return services;
        }
    }
}
