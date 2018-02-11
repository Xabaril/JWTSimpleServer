using JWTSimpleServer.Abstractions;
using JWTSimpleServer.GrantTypes;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace JWTSimpleServer
{
    public class JwtSimpleServerMiddleware
    {
        private readonly JwtSimpleServerOptions _serverOptions;
        private readonly RequestDelegate _next;
        private JwtTokenEncoder _jwtTokenEncoder;

        public JwtSimpleServerMiddleware(
            RequestDelegate next,
            JwtSimpleServerOptions serverOptions)
        {
            _next = next;
            _serverOptions = serverOptions;
            
        }
        public async Task InvokeAsync(
            HttpContext context, 
            IAuthenticationProvider authenticationProvider,
            IRefreshTokenStore refreshTokenStore)
        {
            _jwtTokenEncoder = new JwtTokenEncoder(_serverOptions, refreshTokenStore);
            await ProcessGrantType(
                JwtGrantTypesParser.Parse(context),
                context,
                authenticationProvider ?? throw new Exception(ServerMessages.AuthenticationProviderNotRegistered),
                refreshTokenStore);
        }

        private Task ProcessGrantType(
            IGrantType grantType,
            HttpContext context,
            IAuthenticationProvider authenticationProvider,
            IRefreshTokenStore refreshTokenStore)
        {
            switch (grantType)
            {
                case PasswordGrantType password:
                    return GenerateJwtToken(password, context, authenticationProvider);

                case RefreshTokenGrantType refresh:
                    return RefreshToken(refresh, context, refreshTokenStore);

                default:
                    return WriteResponseError(context, ServerMessages.InvalidGrantType);
            }
        }

        private async Task GenerateJwtToken(
            PasswordGrantType passwordGrandType,
            HttpContext context,
            IAuthenticationProvider authenticationProvider)
        {
            var simpleServerContext = JwtSimpleServerContext.Create(passwordGrandType.UserName, passwordGrandType.Password);
            await authenticationProvider.ValidateClientAuthentication(simpleServerContext);

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

        public async Task RefreshToken(
            RefreshTokenGrantType refreshTokenGrant,
            HttpContext context,
            IRefreshTokenStore refreshTokenStore)
        {
            if (IsRefreshTokenStoreRegistered(refreshTokenStore))
            {
                var token = await refreshTokenStore.GetTokenAsync(refreshTokenGrant.RefreshToken);
                if (token == null)
                {
                    await WriteResponseAsync(StatusCodes.Status404NotFound, context);
                }
                else
                {
                    var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token.AccessToken);
                    var jwtToken = await _jwtTokenEncoder.WriteToken(jwtSecurityToken.Claims.ToList());
                    await refreshTokenStore.InvalidateRefreshTokenAsync(token.RefreshToken);
                    await WriteResponseAsync(StatusCodes.Status200OK, context, JsonConvert.SerializeObject(jwtToken));
                }
            }
            else
            {
                await WriteNoContentResponse(context, ServerMessages.NoRefreshTokenStoreRegistered);
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

        private Task WriteAuthenticationError(HttpContext context, JwtSimpleServerContext jwtSimpleServerContext)
        {
            return WriteResponseError(context, jwtSimpleServerContext.GetError());
        }

        private Task WriteResponseError(HttpContext context, string error)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return context.Response.WriteAsync(error);
        }

        private bool IsRefreshTokenStoreRegistered(IRefreshTokenStore refreshTokenStore)
        {
            return !(refreshTokenStore is NoRefreshTokenStore);
        }
    }
}

