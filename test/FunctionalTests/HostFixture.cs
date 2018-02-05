using JWTSimpleServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalTests
{
    public class HostFixture
    {
        public TestServer Server { get; }
        public HostFixture()
        {
            var builder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddJwtSimpleServer(setup => { });
                }).Configure(app =>
               {
                   app.UseJwtSimpleServer(serverSetup => { });
               });

            Server = new TestServer(builder);
        }
    }   
}
