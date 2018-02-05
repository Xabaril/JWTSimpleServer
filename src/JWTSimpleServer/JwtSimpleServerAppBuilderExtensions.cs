using JWTSimpleServer;

namespace Microsoft.AspNetCore.Builder
{
    public static class JwtSimpleServerAppBuilderExtensions
    {
        public static IApplicationBuilder UseJwtSimpleServer(this IApplicationBuilder app)
        {
            app.UseMiddleware<JwtSimpleServerMiddleware>();
            return app;
        }
    }
}
