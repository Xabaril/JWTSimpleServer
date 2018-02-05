using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWTSimpleServer
{
    public interface IAuthenticationProvider
    {
        Task ValidateClientAuthentication(JwtSimpleServerContext context);
    }
}
