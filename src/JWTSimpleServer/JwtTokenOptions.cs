using JWTSimpleServer.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWTSimpleServer
{
    public class JwtTokenOptions
    { 
        public string ValidIssuer { get; set; } = Constants.DefaultIssuer;
        public string IssuerSigningKey { get; set; } = Constants.DefaultSigningKey;
    }
}
