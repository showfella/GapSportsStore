using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using GapSportsStore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;

namespace GapSportsStore.Models
{

    public class Startup
    {
        public Startup(IConfiguration confguration) => Configuration = confguration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer(Configuration["Data:GapSportStoreProducts:AzConnectionString"]));

            services.AddDbContext<AppIdentityDbContext>(options =>
                    options.UseSqlServer(Configuration["Data:GapSportStoreIdentity:AzConnectionString"]));
            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppIdentityDbContext>()
                    .AddDefaultTokenProviders();

            services.AddTransient<IProductRepository, EFProductRepository>(); //FakeProductRepository>();

            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IOrderRepository, EFOrderRepository>();

            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddMemoryCache();

            services.AddSession();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseMvc(routes =>

            {
                routes.MapRoute(
                    name: null,
                    template: "{category}/Page{productPage:int}",
                    defaults: new { controller = "Product", action = "List" }
                );

                routes.MapRoute(
                    name: null,
                    template: "Page{productPage:int}",
                    defaults: new { controller = "Product", action = "List", productPage = 1 }
                );

                routes.MapRoute(
                name: null,
                template: "{category}", defaults: new { controller = "Product", action = "List", productPage = 1 }
                );

                routes.MapRoute(
                name: null,
                template: "",
                defaults: new { controller = "Product", action = "List", productPage = 1 }
                );

                routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");


            });
            //SeedData.EnsurePopulated(app);
            //IdentitySeedData.EnsurePopulated(app);
        }
    }
}