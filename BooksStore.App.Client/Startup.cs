using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooksStore.App.Client.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BooksStore.App.Client
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRepositories(_configuration);
            services.AddCommandsAndQueries();
            services.AddServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("catpage",
                    "{category}/Page{page:int}",
                    new { Controller = "Store", action = "ShowBooks" });
                endpoints.MapControllerRoute("page", "Page{page:int}",
                    new { Controller = "Store", action = "ShowBooks", page = 1 });
                endpoints.MapControllerRoute("category", "{category}",
                    new { Controller = "Store", action = "ShowBooks", page = 1 });

                endpoints.MapControllerRoute("pagination",
                    "Books/Page{page}",
                    new { Controller = "Store", action = "ShowBooks" });

                endpoints.MapControllerRoute(
                    name: "defalult",
                    pattern: "{controller=Store}/{action=ShowBooks}");
            });
        }
    }
}
