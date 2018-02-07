using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JWTSimpleServer.Abstractions
{
    public interface IRefreshTokenStore
    {
       Task<string> GetRefreshTokenAsync(string refreshToken);
       Task StoreTokenAsync(Token token);
       Task InvalidateRefreshToken(string refreshToken);
       string GenerateRefreshToken();
    }
}
