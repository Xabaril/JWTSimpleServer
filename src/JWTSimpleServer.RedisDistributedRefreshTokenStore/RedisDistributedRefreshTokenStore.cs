using JWTSimpleServer;
using JWTSimpleServer.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public class RedisDistributedRefreshTokenStore : IRefreshTokenStore
    {
        private readonly IDistributedCache _distributedCache;
        private const string TOKEN_CACHE_KEY = "JWT_SERVER_TOKEN_";

        public RedisDistributedRefreshTokenStore(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<Token> GetTokenAsync(string refreshToken)
        {
            var token = await _distributedCache.GetStringAsync($"{TOKEN_CACHE_KEY}{refreshToken}");
            return string.IsNullOrEmpty(token) ? null : JsonConvert.DeserializeObject<Token>(token);
        }

        public Task InvalidateRefreshTokenAsync(string refreshToken)
        {
            return _distributedCache.RemoveAsync($"{TOKEN_CACHE_KEY}{refreshToken}");
        }

        public Task StoreTokenAsync(Token token)
        {
            var serializedToken = JsonConvert.SerializeObject(token);
            return _distributedCache.SetStringAsync($"{TOKEN_CACHE_KEY}{token.RefreshToken}", serializedToken);
        }
        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

    }
}
