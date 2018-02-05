using System;
using System.Collections.Generic;
using System.Text;

namespace JWTSimpleServer
{
    public sealed class ServerMessages
    {
        public const string InvalidGrantType = "Invalid grant_type";
        public const string AuthenticationProviderNotRegistered = "No IAuthenticationProvider service was registered.Please register an IAuthentication Provider implementation";
    }
}
