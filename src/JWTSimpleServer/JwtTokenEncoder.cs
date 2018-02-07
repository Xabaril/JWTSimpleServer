using JWTSimpleServer.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<JwtTokenResponse> WriteToken(IEnumerable<Claim> claims)
        {
            var expiresDate = _options.Expires();
            var startingDate = _options.NotBefore();

            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                claims: claims,
                notBefore: startingDate,
                expires: expiresDate,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(_options.IssuerSigningKey)),
                        SecurityAlgorithms.HmacSha256
                    )
                );

            var encodedAccessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
            var refreshToken = _refreshTokenStore.GenerateRefreshToken();

            await _refreshTokenStore.StoreTokenAsync(
                Token.Create(encodedAccessToken, refreshToken, DateTime.UtcNow)
            );

            return new JwtTokenResponse()
            {
                AccessToken = encodedAccessToken,
                ExpiresIn = GetTokenExpiral(startingDate, expiresDate),
                RefreshToken = refreshToken
            };
        }
        private int GetTokenExpiral(DateTime startingDate, DateTime expiryDate) =>
            Convert.ToInt32((expiryDate.ToUniversalTime() - startingDate.ToUniversalTime()).TotalSeconds);

    }
}
