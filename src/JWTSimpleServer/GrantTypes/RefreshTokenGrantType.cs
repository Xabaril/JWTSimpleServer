using System;
using System.Collections.Generic;
using System.Text;

namespace JWTSimpleServer.GrantTypes
{
    public class RefreshTokenGrantType: IGrantType
    {      
        public string RefreshToken { get; set; }
    }
}
