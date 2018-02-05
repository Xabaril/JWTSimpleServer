using JWTSimpleServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        // Study what will be default implementation and what will be configured by user
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuer = true,
                            ValidIssuer = jwtTokenOptions.ValidIssuer,
                            ValidateAudience = false,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenOptions.IssuerSigningKey)),
                            RequireExpirationTime = true,
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero
                        };
                    });
            
            return services;
        }
    }
}
