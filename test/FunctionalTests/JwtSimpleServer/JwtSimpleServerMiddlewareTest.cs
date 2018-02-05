using FluentAssertions;
using JWTSimpleServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

            var formValues = new Dictionary<string, string>
            {
                { "grant_type" , "foo"}
            };
         
            var form = new FormUrlEncodedContent(formValues);

            var response = await server.CreateClient().PostAsync("/token", form);            
            var content = await response.Content.ReadAsStringAsync();

            content.Should().Be(ServerMessages.InvalidGrantType);
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }    
        
    }
}
