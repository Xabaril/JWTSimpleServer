using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using JWTSimpleServer.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace JWTSimpleServer.InMemoryRefreshTokenStore
{
    public class InMemoryRefreshTokenStore : IRefreshTokenStore
    {
        private readonly IMemoryCache _cache;
        private const string TOKEN_CACHE_KEY = "JWT_SERVER_TOKEN_";

        public InMemoryRefreshTokenStore(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<Token> GetTokenAsync(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return null;
            }
            var token = _cache.Get<Token>($"{TOKEN_CACHE_KEY}{refreshToken}");            
            return Task.FromResult(token);
        }

        public Task InvalidateRefreshTokenAsync(string refreshToken)
        {
            _cache.Remove($"{TOKEN_CACHE_KEY}{refreshToken}");            
            return Task.CompletedTask;
        }

        public Task StoreTokenAsync(Token token)
        {
            _cache.Set($"{TOKEN_CACHE_KEY}{token.RefreshToken}", token);            
            return Task.CompletedTask;
        }
        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
