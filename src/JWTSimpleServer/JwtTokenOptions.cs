using JWTSimpleServer.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWTSimpleServer
{
    public class JwtTokenOptions
    {

        public string ValidIssuer { get; set; } = Constants.DefaultIssuer;
        public bool ValidateIssuer { get; set; } = true;
        public string IssuerSigningKey { get; set; }
        public bool ValidateAudience { get; set; } = false;
        public bool ValidateIssuerSigningKey { get; set; } = true;
        public bool RequireExpirationTime { get; set; } = true;
        public bool ValidateLifetime { get; set; } = true;
        public string ValidAudience { get; set; }
        public TimeSpan ClockSkew { get; set; } = TimeSpan.FromSeconds(300);

        internal TokenValidationParameters BuildTokenValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidAudience = ValidAudience,
                ValidIssuer = ValidIssuer,
                ValidateAudience = ValidateAudience,
                ValidateIssuerSigningKey = ValidateIssuerSigningKey,
                RequireExpirationTime = RequireExpirationTime,
                ValidateLifetime = ValidateLifetime,
                ValidateIssuer = ValidateIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(IssuerSigningKey)),
                ClockSkew = ClockSkew
            };             
        }
    }
}
