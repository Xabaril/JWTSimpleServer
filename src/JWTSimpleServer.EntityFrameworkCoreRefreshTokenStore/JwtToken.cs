using System;
using System.Collections.Generic;
using System.Text;

namespace JWTSimpleServer.EntityFrameworkCoreRefreshTokenStore
{
    public class JwtToken : Token
    {
        public int Id { get; set; }

        public static JwtToken CopyFrom(Token token)
        {
            return new JwtToken
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                CreatedAt = token.CreatedAt
            };
        }
    }
}
