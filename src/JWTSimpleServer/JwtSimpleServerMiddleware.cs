using JWTSimpleServer.GrantTypes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JWTSimpleServer
{
    public class JwtSimpleServerMiddleware : IMiddleware
    {
        private readonly JwtSimpleServerOptions _serverOptions;
        private readonly IAuthenticationProvider _authenticationProvider;
        public JwtSimpleServerMiddleware(IAuthenticationProvider authenticationProvider)
        {
            _authenticationProvider = authenticationProvider ??
                                     throw new Exception("No IAuthenticationProvider service was registered. Please register an IAuthentication Provider implementation");
        }

        public JwtSimpleServerMiddleware(JwtSimpleServerOptions serverOptions)
        {
            _serverOptions = serverOptions;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await ProcessGrantType(JwtGrantTypesParser.Parse(context), context);

        }

        private async Task PasswordJwtEncode(PasswordGrantType passwordGrandType, HttpContext context)
        {
            var simpleServerContext = JwtSimpleServerContext.Create(passwordGrandType.UserName, passwordGrandType.Password);
            await _authenticationProvider.ValidateClientAuthentication(simpleServerContext);

            if (simpleServerContext.IsValid())
            {

            }
            else
            {
                await WriteAuthenticationError(context, simpleServerContext);
            }
        }

        private Task ProcessGrantType(IGrantType grantType, HttpContext context)
        {
            switch (grantType)
            {
                case PasswordGrantType password:
                    return PasswordJwtEncode(password, context);

                case RefreshTokenGrantType refresh:
                    return Task.CompletedTask;

                default:
                    return WriteResponseError(context, "Invalid grant_type");
            }
        }

        private Task WriteAuthenticationError(HttpContext context, JwtSimpleServerContext jwtSimpleServerContext)
        {
            return WriteResponseError(context, jwtSimpleServerContext.GetError());
        }

        private Task WriteResponseError(HttpContext context, string error)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return context.Response.WriteAsync(error);
        }
    }
}

