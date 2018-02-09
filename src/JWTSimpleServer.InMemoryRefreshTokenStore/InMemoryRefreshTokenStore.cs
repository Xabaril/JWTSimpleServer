using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using JWTSimpleServer.Abstractions;


namespace JWTSimpleServer.InMemoryRefreshTokenStore
{
    public class InMemoryRefreshTokenStore : IRefreshTokenStore
    {
        private readonly ConcurrentDictionary<string, Token> _tokens = new ConcurrentDictionary<string, Token>();

        public Task<Token> GetTokenAsync(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return null;
            }
            var token = _tokens[refreshToken];
            return Task.FromResult(token);
        }

        public Task InvalidateRefreshTokenAsync(string refreshToken)
        {
            _tokens.TryRemove(refreshToken, out var token);
            return Task.CompletedTask;
        }

        public Task StoreTokenAsync(Token token)
        {
            _tokens.TryAdd(token.RefreshToken, token);
            return Task.CompletedTask;
        }
        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
