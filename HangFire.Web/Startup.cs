using Hangfire;
using HangFire.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangFire.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) // servisleri eklediðimiz yer.
        {
            // uygulamanýn herhangi bir yerinden IEmailSender görürsen, EmailSender'dan nesne örneði al.
            services.AddScoped<IEmailSender, EmailSender>();

            // database connection string'imizi ekledik.
            services.AddHangfire(config =>
                config.UseSqlServerStorage(Configuration.GetConnectionString("HangFireConnection")));

            services.AddHangfireServer(); //server servisini ekledik.

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) // middleware yapýlarýmýzý eklediðimiz yer.
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // www.mysite.com/hangfire
            app.UseHangfireDashboard("/hangfire"); // ayarlamalarýn gözüktüðü dashboard var onu ekledik.

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
