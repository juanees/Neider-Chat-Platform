using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Eureka;
using Ocelot.Cache.CacheManager;
using Microsoft.Extensions.Configuration;

namespace Neider.Gateway
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOcelot().AddEureka().AddCacheManager(x => x.WithDictionaryHandle());
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            var appSettings = new AppSettings();
            app.ApplicationServices.GetService<IConfiguration>()
                .GetSection("AppSettings")
                .Bind(appSettings);

            //app.UseCors(b => b
            //           .WithOrigins(appSettings.AllowedChatOrigins)
            //           .AllowAnyMethod()
            //           .AllowAnyHeader()
            //           .AllowCredentials()
            //        );

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("hello world from gateway!");
                });
            });

            app.UseOcelot().Wait();
        }
    }
}
