using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTSimpleServer.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleServerInMemoryStorage
{
    public class Startup
    {
        public const string SigningKey = "InMemorySampleSigningKey";
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAuthenticationProvider, CustomAuthenticationProvider>()
            .AddCors()
            .AddJwtSimpleServer(setup =>
            {
                setup.IssuerSigningKey = SigningKey;
            })
            .AddJwtInMemoryRefreshTokenStore()
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
            }, pipeline => {

                pipeline.UseCors(setup =>
                {
                    setup.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            app.UseMvc();
        }
    }
}
