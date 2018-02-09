using System;
using System.Collections.Generic;
using System.Text;

namespace JWTSimpleServer
{
    public class Token
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime CreatedAt { get; set; } 

        public static Token Create(string accessToken, string refreshToken, DateTime createdAt)
        {
            return new Token
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                CreatedAt = createdAt
            };
        }
    }
}
