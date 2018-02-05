using JWTSimpleServer.GrantTypes;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JWTSimpleServer
{
    public class JwtSimpleServerMiddleware 
    {
        private readonly JwtSimpleServerOptions _serverOptions;
        private readonly IAuthenticationProvider _authenticationProvider;
        private readonly RequestDelegate _next;

        public JwtSimpleServerMiddleware(RequestDelegate next, 
            IAuthenticationProvider authenticationProvider, JwtSimpleServerOptions serverOptions)
        {
            _next = next;
            _authenticationProvider = authenticationProvider ?? throw new Exception(ServerMessages.AuthenticationProviderNotRegistered);
            _serverOptions = serverOptions;
        }      
        public async Task InvokeAsync(HttpContext context)
        {
            await ProcessGrantType(JwtGrantTypesParser.Parse(context), context);
        }

        private async Task PasswordJwtEncode(PasswordGrantType passwordGrandType, HttpContext context)
        {
            var simpleServerContext = JwtSimpleServerContext.Create(passwordGrandType.UserName, passwordGrandType.Password);
            await _authenticationProvider.ValidateClientAuthentication(simpleServerContext);

            if (simpleServerContext.IsValid())
            {
                var jwtToken = new JwtTokenEncoder(_serverOptions).WriteToken(simpleServerContext);
                await WriteResponseAsync(context, JsonConvert.SerializeObject(jwtToken));
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
                    return WriteResponseError(context, ServerMessages.InvalidGrantType);
            }
        }

        
        private Task WriteResponseAsync(
         HttpContext context,
         string content)
        {
            context.Response.Headers["Content-Type"] = "application/json";         
            context.Response.StatusCode = StatusCodes.Status200OK;
            return context.Response.WriteAsync(content);
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

