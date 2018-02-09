using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTSimpleServer.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SimpleServerEFCoreStorage
{
    public class Startup
    {
        public const string SigningKey = "InMemorySampleSigningKey";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddScoped<IAuthenticationProvider, CustomAuthenticationProvider>()
                .AddJwtSimpleServer(options => options.IssuerSigningKey = SigningKey)
                .AddJwtEntityFrameworkCoreRefreshTokenStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    {
                        builder.UseSqlServer(
                            Configuration["ConnectionStrings:DefaultConnection"],
                            sqlServerOptions => sqlServerOptions.MigrationsAssembly(typeof(Startup).Assembly.FullName));
                    };
                })
                .AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app
                .UseDefaultFiles()
                .UseStaticFiles()
                .UseJwtSimpleServer(setup =>
                {
                    setup.IssuerSigningKey = SigningKey;
                })
                .UseMvc();
        }
    }
}
