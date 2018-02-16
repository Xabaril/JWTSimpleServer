using JWTSimpleServer;
using JWTSimpleServer.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class JwtSimpleServerServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtSimpleServer(this IServiceCollection services, Action<JwtTokenOptions> setup)
        {
            var jwtTokenOptions = new JwtTokenOptions();
            setup(jwtTokenOptions);

            if (string.IsNullOrEmpty(jwtTokenOptions.IssuerSigningKey))
            {
                throw new ArgumentNullException(nameof(jwtTokenOptions.IssuerSigningKey));
            };

            var tokenValidationParameters = jwtTokenOptions.BuildTokenValidationParameters();

            services.TryAddSingleton<IRefreshTokenStore, NoRefreshTokenStore>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {                                                                                                                          
                       options.TokenValidationParameters = tokenValidationParameters;
                    });
            
            return services;
        }
    }
}
