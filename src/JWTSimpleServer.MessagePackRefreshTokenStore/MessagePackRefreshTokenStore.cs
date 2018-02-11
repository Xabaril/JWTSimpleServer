using JWTSimpleServer.Abstractions;
using MessagePack;
using System;
using MessagePack;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Concurrent;
using System.Threading;

namespace JWTSimpleServer.MessagePackRefreshTokenStore
{
    public class MessagePackRefreshTokenStore : IRefreshTokenStore
    {
        private readonly JwtStoreOptions _storeOptions;
        private readonly SemaphoreSlim _writeSemaphore = new SemaphoreSlim(1, 1);
        public MessagePackRefreshTokenStore(JwtStoreOptions storeOptions)
        {
            _storeOptions = storeOptions;
        }

        public async Task<Token> GetTokenAsync(string refreshToken)
        {
            var tokenStore = await ReadBinaryStoreAsync();
            if (tokenStore.ContainsKey(refreshToken))
            {
                return tokenStore[refreshToken];
            }
            return null;
        }

        public async Task InvalidateRefreshTokenAsync(string refreshToken)
        {
            var tokenStore = await ReadBinaryStoreAsync();
            tokenStore.TryRemove(refreshToken, out Token token);
            await WriteBinaryStoreAsync(tokenStore);
        }

        public async Task StoreTokenAsync(Token token)
        {
            var tokenStore = await ReadBinaryStoreAsync();
            tokenStore.TryAdd(token.RefreshToken, token);
            await WriteBinaryStoreAsync(tokenStore);
        }

        private Task<ConcurrentDictionary<string, Token>> ReadBinaryStoreAsync()
        {
            EnsureStore();
            using (var binaryStore = new FileStream(_storeOptions.Path, FileMode.Open))
            {
                return MessagePackSerializer.DeserializeAsync<ConcurrentDictionary<string, Token>>(binaryStore);
            }
        }

        private async Task WriteBinaryStoreAsync(ConcurrentDictionary<string, Token> store)
        {
            await _writeSemaphore.WaitAsync();
            try
            {
                using (var binaryStore = new FileStream(_storeOptions.Path, FileMode.Append))
                {
                    var content = MessagePackSerializer.Serialize(binaryStore);
                    await binaryStore.WriteAsync(content, 0, content.Length);
                }
            }
            finally
            {
                _writeSemaphore.Release();
            }
        }

        private void EnsureStore()
        {
            if (!File.Exists(_storeOptions.Path))
            {
                throw new Exception($"{nameof(MessagePackRefreshTokenStore)} : Error Trying to retrieve data. The store does not exist");
            }
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString().Replace("-", String.Empty);
        }
    }
}
