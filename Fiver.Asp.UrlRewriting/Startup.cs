using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Rewrite;

namespace Fiver.Asp.UrlRewriting
{
    public class Startup
    {
        public void ConfigureServices(
            IServiceCollection services)
        {
        }

        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env)
        {
            var rewrite = new RewriteOptions()
                .AddRedirect("films", "movies")
                .AddRewrite("actors", "stars", true);

            //var rewrite = new RewriteOptions()
            //    .AddRedirectToHttps(302, 7229);

            //var rewrite = new RewriteOptions()
            //    .Add(new MoviesRedirectRule(
            //            matchPaths: new[] { "/films", "/features", "/albums" },
            //            newPath: "/movies"));

            app.UseRewriter(rewrite);

            app.Run(async context =>
            {
                var path = context.Request.Path;
                var query = context.Request.QueryString;
                await context.Response.WriteAsync($"New URL: {path}{query}");
            });
        }
    }
}
