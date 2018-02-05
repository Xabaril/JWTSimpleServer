using FluentAssertions;
using JWTSimpleServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.JwtSimpleServer
{
   
    public class JwtSimpleServerMiddlewareTestShould
    {        
        [Fact]
        public async Task reject_invalid_grant_type()
        {
            var server = new TestServerBuilder()
                     .WithSuccessAuthentication()
                     .Build();
                     
            var response = await server.CreateClient().PostAsync("/token", GrantTypes.AnInvalidGrantType());            
            var content = await response.Content.ReadAsStringAsync();

            content.Should().Be(ServerMessages.InvalidGrantType);
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }    

        [Fact]
        public async Task return_a_valid_jwt_token_when_grant_type_password()
        {
            var server = new TestServerBuilder()
                   .WithSuccessAuthentication()
                   .Build();

            var response = await server.CreateClient().PostAsync("/token", GrantTypes.APasswordGrantType());
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var jwtTokenResponse = JsonConvert.DeserializeObject<JwtTokenResponse>(content);

            jwtTokenResponse.Should().NotBeNull();
            jwtTokenResponse.AccessToken.Should().NotBeNull();
            jwtTokenResponse.ExpiresIn.Should().BeGreaterThan(0);
           
        }
        
    }
}
