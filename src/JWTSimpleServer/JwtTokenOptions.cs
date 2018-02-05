using System;
using System.Collections.Generic;
using System.Text;

namespace JWTSimpleServer
{
    public class JwtTokenOptions
    {        
        public string ValidIssuer { get; set; }   
        public string IssuerSigningKey { get; set; }        
    }
}
