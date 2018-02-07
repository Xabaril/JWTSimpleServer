using JWTSimpleServer.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JWTSimpleServer
{
    public class NoRefreshTokenStore : IRefreshTokenStore
    {
        public Task<string> GetRefreshTokenAsync(string refreshToken)
        {
            return Task.FromResult(string.Empty);
        }

        public Task InvalidateRefreshToken(string refreshToken)
        {
            return Task.CompletedTask;
        }

        public Task StoreTokenAsync(string accessToken, string refreshToken)
        {
            return Task.CompletedTask;
        }
    }
}
