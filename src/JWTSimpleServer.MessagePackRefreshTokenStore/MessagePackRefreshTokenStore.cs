using JWTSimpleServer.Abstractions;
using MessagePack;
using System;
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
        private readonly SemaphoreSlim _readSemaphore = new SemaphoreSlim(1, 1);
        public MessagePackRefreshTokenStore(JwtStoreOptions storeOptions)
        {
            _storeOptions = storeOptions;
            EnsureStore();
        }

        public async Task<Token> GetTokenAsync(string refreshToken)
        {
            var tokenStore = await ReadBinaryStoreAsync();
            if (tokenStore.ContainsKey(refreshToken))
            {
                return tokenStore[refreshToken].CopyTo();
            }
            return null;
        }

        public async Task InvalidateRefreshTokenAsync(string refreshToken)
        {
            var tokenStore = await ReadBinaryStoreAsync();
            tokenStore.TryRemove(refreshToken, out JwtToken token);
            await WriteBinaryStoreAsync(tokenStore);
        }

        public async Task StoreTokenAsync(Token token)
        {
            var tokenStore = await ReadBinaryStoreAsync();
            tokenStore.TryAdd(token.RefreshToken, JwtToken.CopyFrom(token));
            await WriteBinaryStoreAsync(tokenStore);
        }

        private async Task<ConcurrentDictionary<string, JwtToken>> ReadBinaryStoreAsync()
        {
            await _readSemaphore.WaitAsync();
            try
            {
                using (var binaryStore = new FileStream(_storeOptions.Path, FileMode.Open))
                {
                    var bytes = new byte[binaryStore.Length];
                    await binaryStore.ReadAsync(bytes, 0, (int)bytes.Length);
                    return MessagePackSerializer.Deserialize<ConcurrentDictionary<string, JwtToken>>(bytes);
                }
            }
            finally
            {
                _readSemaphore.Release();
            }
        }

        private async Task WriteBinaryStoreAsync(ConcurrentDictionary<string, JwtToken> store)
        {
            await _writeSemaphore.WaitAsync();
            try
            {
                var content = MessagePackSerializer.Serialize(store);
                using(var binaryStore = new FileStream(_storeOptions.Path, FileMode.Create))
                {
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
                using (var fs = File.Create(_storeOptions.Path)) { };
                var initialDictionary = new ConcurrentDictionary<string, JwtToken>();
                WriteBinaryStoreAsync(initialDictionary).GetAwaiter().GetResult();
            }
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString().Replace("-", String.Empty);
        }
    }
}
