using JWTSimpleServer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System.Linq;
using JWTSimpleServer.Abstractions;

namespace FunctionalTests
{
    public class TestServerBuilder
    {
        private IAuthenticationProvider authProvider;
        public TestServerBuilder WithSuccessAuthentication()
        {
            authProvider = new FakeSuccessAuthenticationProvider();
            return this;
        }
        
        public TestServer Build()
        {
            var webhostBuilder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddJwtSimpleServer(options => { })                                           
                    .AddTransient(ctx =>
                    {
                        return authProvider;
                    })
                    .AddMvcCore()
                    .AddAuthorization();

                }).Configure(app =>
               {
                   app.UseJwtSimpleServer(setup => {
                       
                   })
                   .UseMvc();
               });

            return new TestServer(webhostBuilder);
        }

        internal class FakeSuccessAuthenticationProvider : IAuthenticationProvider
        {
            public Task ValidateClientAuthentication(JwtSimpleServerContext context)
            {
                context.Success(Enumerable.Empty<Claim>());
                return Task.CompletedTask;
            }
        }
    }


}
