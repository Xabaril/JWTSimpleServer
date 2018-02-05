
using JWTSimpleServer.GrantTypes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace JWTSimpleServer
{
    public class JwtGrantTypesParser
    {
        public static IGrantType Parse(HttpContext context)
        {
            return ParseRequest(context.Request);
        }
        private static IGrantType ParseRequest(HttpRequest request)
        {
            var requestForm = request.Form;

            if (!requestForm.ContainsKey(GrantType.Password))
            {
                return new PasswordGrantType()
                {
                    UserName = requestForm["username"],
                    Password = requestForm["password"]
                };
            }
            else if (!requestForm.ContainsKey(GrantType.RefreshToken))
            {
                throw new NotImplementedException();
            }
            else
            {
                return new InvalidGrantType();
            }
        }
    }
}
