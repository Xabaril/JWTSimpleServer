using JWTSimpleServer.Abstractions;
using JWTSimpleServer.GrantTypes;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace JWTSimpleServer
{
    public class JwtSimpleServerMiddleware
    {
        private readonly JwtSimpleServerOptions _serverOptions;
        private readonly IAuthenticationProvider _authenticationProvider;
        private readonly IRefreshTokenStore _refreshTokenStore;
        private readonly RequestDelegate _next;
        private readonly JwtTokenEncoder _jwtTokenEncoder;

        public JwtSimpleServerMiddleware(RequestDelegate next,
            IAuthenticationProvider authenticationProvider,
            IRefreshTokenStore refreshTokenStore,
            JwtSimpleServerOptions serverOptions)
        {
            _next = next;
            _authenticationProvider = authenticationProvider ?? throw new Exception(ServerMessages.AuthenticationProviderNotRegistered);
            _refreshTokenStore = refreshTokenStore;
            _serverOptions = serverOptions;
            _jwtTokenEncoder = new JwtTokenEncoder(_serverOptions, _refreshTokenStore);
        }
        public async Task InvokeAsync(HttpContext context)
        {
            await ProcessGrantType(JwtGrantTypesParser.Parse(context), context);
        }

        private async Task GenerateJwtToken(PasswordGrantType passwordGrandType, HttpContext context)
        {
            var simpleServerContext = JwtSimpleServerContext.Create(passwordGrandType.UserName, passwordGrandType.Password);
            await _authenticationProvider.ValidateClientAuthentication(simpleServerContext);

            if (simpleServerContext.IsValid())
            {
                var jwtToken = await _jwtTokenEncoder.WriteToken(simpleServerContext.Claims);

                await WriteResponseAsync(StatusCodes.Status200OK, context, JsonConvert.SerializeObject(jwtToken));
            }
            else
            {
                await WriteAuthenticationError(context, simpleServerContext);
            }
        }

        public async Task RefreshToken(RefreshTokenGrantType refreshTokenGrant, HttpContext context)
        {
            if (IsRefreshTokenStoreRegistered())
            {
                var token = await _refreshTokenStore.GetTokenAsync(refreshTokenGrant.RefreshToken);
                if (token == null)
                {
                    await WriteResponseAsync(StatusCodes.Status404NotFound, context);
                }
                else
                {
                    var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token.AccessToken);
                    var jwtToken = await _jwtTokenEncoder.WriteToken(jwtSecurityToken.Claims.ToList());
                    await _refreshTokenStore.InvalidateRefreshTokenAsync(token.RefreshToken);
                    await WriteResponseAsync(StatusCodes.Status200OK, context, JsonConvert.SerializeObject(jwtToken));
                }
            }
            else
            {
                await WriteNoContentResponse(context, ServerMessages.NoRefreshTokenStoreRegistered);
            }            
        }

        private Task ProcessGrantType(IGrantType grantType, HttpContext context)
        {
            switch (grantType)
            {
                case PasswordGrantType password:
                    return GenerateJwtToken(password, context);

                case RefreshTokenGrantType refresh:
                    return RefreshToken(refresh, context);

                default:
                    return WriteResponseError(context, ServerMessages.InvalidGrantType);
            }
        }
        private Task WriteResponseAsync(
         int statusCode,
         HttpContext context,
         string content = "",
         string contentType = "application/json")
        {
            context.Response.Headers["Content-Type"] = contentType;
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(content);
        }

        private Task WriteNoContentResponse(
            HttpContext context,
            string content)
        {
            return WriteResponseAsync(
                StatusCodes.Status204NoContent,
                context,
                content,
                MediaTypeNames.Text.Plain);

        }
        private bool IsRefreshTokenStoreRegistered()
        {
            return !(_refreshTokenStore is NoRefreshTokenStore);
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

