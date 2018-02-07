using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWTSimpleServer.Abstractions
{
    public interface IAuthenticationProvider
    {
        Task ValidateClientAuthentication(JwtSimpleServerContext context);
    }
}
