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
using Smidge;
using Smidge.Options;
using Smidge.Cache;

namespace SmidgeApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSmidge(Configuration.GetSection("smidge")); // smidge kutuphanesinin servisini ekledik.

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) //middleware eklenen yer.
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

            app.UseRouting();

            app.UseAuthorization();

            //smidge kütüphanesi middleware yapısı. 

            app.UseSmidge(bundle =>
            {
                // EnableCompositeProcessing diyerek debug'da dahi olsa birleştirme işlemini gerçekleştir. EnableFileWatcher oluşturdugum bundle'da dosyaları izle, bir değişiklik olursa yeniden oluştur. SetCacheBusterType<AppDomainLifetimeCacheBuster> ile uygulama ayağa kalkınca cache'i boz ve yeniden oluştur.
                // browser'a oluşturulan bundle dosyasını hiç cache'leme demek için de CacheControlOptions giriyoruz. enableEtag: false yapıyoruz. enable tag bir nevi token ve bu token'a göre browser değişiklikleri algılıyor. aynı tag gönderirse header, o zaman browser değişiklik yapmıyor ve cache bellekten ekmeğini yiyor. 304 durum kodu bi şey değişmemiş cache den oku demek.
                // biz false yaptık bu sayede browser cache çalışmıyor ve serverdan bilgileri çekiyor. cacheControlMaxAge: 0 ise browser cache'yi ne kadar tutabilirin saniye cinsinden değeri.
                bundle.CreateJs("my-js-bundle", "~/js/").WithEnvironmentOptions(BundleEnvironmentOptions.Create().ForDebug(builder => builder.EnableCompositeProcessing().EnableFileWatcher().SetCacheBusterType<AppDomainLifetimeCacheBuster>().CacheControlOptions(enableEtag: false, cacheControlMaxAge: 0)).Build());

                bundle.CreateCss("my-css-bundle", "~/css/site.css", "~/lib/bootstrap/dist/css/bootstrap.css");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
