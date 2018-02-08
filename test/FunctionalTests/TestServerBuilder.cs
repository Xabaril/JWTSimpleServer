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
        private bool _useInMemoryStore = false;
        public TestServerBuilder WithSuccessAuthentication()
        {
            authProvider = new FakeSuccessAuthenticationProvider();
            return this;
        }
        
        public TestServerBuilder WithInMemoryStore()
        {
            _useInMemoryStore = true;
            return this;
        }
        public TestServer Build()
        {
            var webhostBuilder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    var serviceCollection = services.AddJwtSimpleServer(options => { })
                    .AddTransient(ctx =>
                    {
                        return authProvider;
                    });

                    if (_useInMemoryStore)
                    {
                        serviceCollection.AddJwtInMemoryRefreshTokenStore();
                    }

                    serviceCollection.AddMvcCore()
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
                context.Success(Enumerable.Empty<Claim>().ToList());
                return Task.CompletedTask;
            }
        }
    }


}
