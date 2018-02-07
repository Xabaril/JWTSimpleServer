using JWTSimpleServer.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JWTSimpleServer
{
    public class NoRefreshTokenStore : IRefreshTokenStore
    { 
        public Task<Token> GetTokenAsync(string refreshToken)
        {
            return null;
        }

        public Task InvalidateRefreshTokenAsync(string refreshToken)
        {
            return Task.CompletedTask;
        }

        public Task StoreTokenAsync(Token token)
        {
            return Task.CompletedTask;
        }
        public string GenerateRefreshToken()
        {
            return string.Empty;
        }
    }
}
