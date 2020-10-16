using BooksStore.App.Client.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddAutoMapper();
            services.AddControllersWithViews();
            services.AddRepositories(_configuration);
            services.AddCommandsAndQueries();
            services.AddServices();
            services.AddDistributedMemoryCache();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRequestLocalization("ru-RU");
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(null,
                    "Publishers/Page{page}/{propertyName}/{order}", new {controller = "Publishers", action = "ShowPublishers"});
                endpoints.MapControllerRoute(null,
                    "Publishers/Page{page}", new {controller = "Publishers", action = "ShowPublishers"});
                endpoints.MapControllerRoute("pub",
                    "Publisher/CreateOrEdit/{Id:long}",
                    new {controller = "Publishers", action = "CreatePublisher"});

                endpoints.MapControllerRoute("catpage",
                    "{category}/Page{page:int}",
                    new { Controller = "Store", action = "ShowBooks" });
                endpoints.MapControllerRoute("page", "Page{page:int}",
                    new { Controller = "Store", action = "ShowBooks", page = 1 });
                endpoints.MapControllerRoute("category", "{category}",
                    new { Controller = "Store", action = "ShowBooks", page = 1 });

                endpoints.MapControllerRoute("storeBookDetails",
                    "Books/Details/{bookId:long}",
                    new { Controller = "Store", action = "ShowDetails" });
                endpoints.MapControllerRoute("pagination",
                    "Books/Page{page}",
                    new { Controller = "Store", action = "ShowBooks" });

                endpoints.MapControllerRoute(
                    name: "defalult",
                    pattern: "{controller=Publishers}/{action=ShowPublishers}");
            });
        }
    }
}
