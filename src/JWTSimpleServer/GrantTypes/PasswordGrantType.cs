using System;
using System.Collections.Generic;
using System.Text;

namespace JWTSimpleServer.GrantTypes
{
    public class PasswordGrantType: IGrantType
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
