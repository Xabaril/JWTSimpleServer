using System;
using System.Collections.Generic;
using System.Text;

namespace JWTSimpleServer.EntityFrameworkCoreRefreshTokenStore
{
    public class JwtToken : Token
    {
        public int Id { get; set; }
    }
}
