using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JWTSimpleServer.Abstractions
{
    public interface IRefreshTokenStore
    {
       Task<Token> GetTokenAsync(string refreshToken);
       Task StoreTokenAsync(Token token);
       Task InvalidateRefreshTokenAsync(string refreshToken);
       string GenerateRefreshToken();
    }
}
