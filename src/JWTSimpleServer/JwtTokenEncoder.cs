using JWTSimpleServer.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JWTSimpleServer
{
    public class JwtTokenEncoder
    {
        private readonly JwtSimpleServerOptions _options;
        private readonly IRefreshTokenStore _refreshTokenStore;

        public JwtTokenEncoder(
            JwtSimpleServerOptions options,
            IRefreshTokenStore refreshTokenStore)
        {
            _options = options;
            _refreshTokenStore = refreshTokenStore;
        }
        public JwtTokenResponse WriteToken(JwtSimpleServerContext context)
        {
            var expiresDate = _options.Expires();
            var startingDate = _options.NotBefore();

            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                claims: context.Claims,
                notBefore: startingDate,
                expires: expiresDate,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(_options.IssuerSigningKey)),
                        SecurityAlgorithms.HmacSha256
                    )
                );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);            

            return new JwtTokenResponse()
            {
                AccessToken = encodedJwt,
                ExpiresIn = GetTokenExpiral(startingDate, expiresDate)
            };
        }
        private int GetTokenExpiral(DateTime startingDate, DateTime expiryDate) =>
            Convert.ToInt32((expiryDate.ToUniversalTime() - startingDate.ToUniversalTime()).TotalSeconds);

    }
}
