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

        public async Task<Token> GetTokenAsync(string refreshToken)
        {
            var token = await context.Tokens.FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);

            if (token != null)
            {
                return Token.Create(token.AccessToken, token.RefreshToken, token.CreatedAt);
            }

            return null;
        }

        public async Task InvalidateRefreshTokenAsync(string refreshToken)
        {
            var token = await context.Tokens.FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);

            if (token != null)
            {
                context.Remove(token);
                await context.SaveChangesAsync();
            }
        }

        public async Task StoreTokenAsync(Token token)
        {
            if (token == null)
            {
                throw new ArgumentException(nameof(token));
            }

            var entity = Token.Create(token.AccessToken, token.RefreshToken, token.CreatedAt);
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString().Replace("-", String.Empty);
        }
    }
}
