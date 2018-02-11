using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWTSimpleServer.MessagePackRefreshTokenStore
{
    [MessagePackObject]
    public class JwtToken
    {
        [Key(0)]
        public string AccessToken { get; set; }
        [Key(1)]
        public string RefreshToken { get; set; }
        [Key(2)]
        public DateTime CreatedAt { get; set; }

        public Token CopyTo()
        {
            return Token.Create(AccessToken, RefreshToken, CreatedAt);
        }

        public static JwtToken CopyFrom(Token token)
        {
            return new JwtToken()
            {
                AccessToken = token.AccessToken,
                RefreshToken = token.RefreshToken,
                CreatedAt = token.CreatedAt
            };
        }
    }
}
