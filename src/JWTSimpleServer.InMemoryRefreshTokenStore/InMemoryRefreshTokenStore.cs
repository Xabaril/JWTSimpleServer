using System.Threading.Tasks;
using JWTSimpleServer.Abstractions;


namespace JWTSimpleServer.InMemoryRefreshTokenStore
{
    public class InMemoryRefreshTokenStore : IRefreshTokenStore
    {
        public string GenerateRefreshToken()
        {
            throw new System.NotImplementedException();
        }

        public Task<Token> GetTokenAsync(string refreshToken)
        {
            throw new System.NotImplementedException();
        }

        public Task InvalidateRefreshTokenAsync(string refreshToken)
        {
            throw new System.NotImplementedException();
        }

        public Task StoreTokenAsync(Token token)
        {
            throw new System.NotImplementedException();
        }
    }
}
