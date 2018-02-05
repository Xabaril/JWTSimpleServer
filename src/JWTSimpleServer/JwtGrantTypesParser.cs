
using JWTSimpleServer.GrantTypes;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
namespace JWTSimpleServer
{
    public class JwtGrantTypesParser
    {
        private const string GrandTypeParameter = "grant_type";

        public static IGrantType Parse(HttpContext context)
        {
            return ParseRequest(context.Request);
        }
        private static IGrantType ParseRequest(HttpRequest request)
        {
            var requestForm = request.Form;

            if (requestForm.ContainsKey(GrandTypeParameter))
            {
                var grandTypeValue = requestForm[GrandTypeParameter].FirstOrDefault();
                switch (grandTypeValue)
                {
                    case GrantType.Password:
                        return new PasswordGrantType()
                        {
                            UserName = requestForm["username"].FirstOrDefault(),
                            Password = requestForm["password"].FirstOrDefault()
                        };

                    case GrantType.RefreshToken:
                        throw new NotImplementedException();
                    default:
                        return new InvalidGrantType();

                }                
            }            
            else
            {
                return new InvalidGrantType();
            }
        }
    }
}
