using JWTSimpleServer.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace JWTSimpleServer.EntityFrameworkCoreRefreshTokenStore
{
    /// <summary>
    /// EF implementation of RefreshTokenStore
    /// </summary>
    public class EntityFrameworkCoreRefreshTokenStore : IRefreshTokenStore
    {
        private readonly JwtSimpleServerDbContext context;

        public EntityFrameworkCoreRefreshTokenStore(JwtSimpleServerDbContext context)
        {
            this.context = context ?? throw new ArgumentException(nameof(context));
        }

        public Task<Token> GetTokenAsync(string refreshToken)
        {
            return context.Tokens.FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);
        }

        public async Task InvalidateRefreshTokenAsync(string refreshToken)
        {
            var token = await GetTokenAsync(refreshToken);

            if (token != null)
            {
                context.Remove(token);
                await context.SaveChangesAsync();
            }
        }

        public async Task StoreTokenAsync(Token token)
        {
            await context.AddAsync(token);
            await context.SaveChangesAsync();
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString().Replace("-", String.Empty);
        }
    }
}
