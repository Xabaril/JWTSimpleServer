using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTSimpleServer.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleServerMessagePackStorage
{
    public class Startup
    {
        public const string SigningKey = "MessagePackSampleSigningKey";
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAuthenticationProvider, CustomAuthenticationProvider>()
            .AddJwtSimpleServer(setup =>
            {
                setup.IssuerSigningKey = SigningKey;
            })
            .AddJwtMessagePackRefreshTokenStore(setup =>
            {
                setup.Path = "MyBinaryStore.bin";
            })
            .AddMvcCore().
            AddAuthorization();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseJwtSimpleServer(setup =>
            {
                setup.IssuerSigningKey = SigningKey;
            });

            app.UseMvc();
        }
    }
}
