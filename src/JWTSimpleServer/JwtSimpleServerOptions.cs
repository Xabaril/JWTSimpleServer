using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JWTSimpleServer
{
    public class JwtSimpleServerOptions
    {
        public string Path { get; set; } = "/token";        
        
        public void UseRefreshToken() => _UseRefreshToken = true;        

        private bool _UseRefreshToken = false;
    }
}
