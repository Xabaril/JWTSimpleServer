using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JWTSimpleServer
{
    public class JwtSimpleServerOptions
    {
        public string Path { get; set; } = "/token";
        public string Issuer { get; set; } = Constants.DefaultIssuer;
        public string IssuerSigningKey { get; set; }
        public Func<DateTime> NotBefore = () => DateTime.UtcNow;
        public Func<DateTime> Expires = () => DateTime.UtcNow.AddMinutes(15);        
        public void UseRefreshToken() => _UseRefreshToken = true;        

        private bool _UseRefreshToken = false;
    }
}
