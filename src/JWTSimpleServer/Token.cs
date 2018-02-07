using System;
using System.Collections.Generic;
using System.Text;

namespace JWTSimpleServer
{
    public class Token
    {
        public string AccessToken { get; }
        public string RefreshToken { get; }
        public DateTime CreatedAt { get; }

        private Token(string accessToken, string refreshToken, DateTime createdAt)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            CreatedAt = createdAt;
        }   

        public static Token Create(string accessToken, string refreshToken, DateTime createdAt)
        {
            return new Token(accessToken, refreshToken, createdAt);
        }
    }
}
