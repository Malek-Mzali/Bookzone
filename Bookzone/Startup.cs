using System;
using Bookzone.Extensions;
using Bookzone.Models.DAL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NToastNotify;

namespace Bookzone
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            #region snippet1    
            services.AddLocalization(options => options.ResourcesPath = "Resources");     
            services.AddMvc()    
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)    
                .AddDataAnnotationsLocalization();    
         
            services.Configure<RequestLocalizationOptions>(options =>    
            {    
                var supportedCultures = new[] { "en-US", "fr-FR", "ar-TN" };     
                options.SetDefaultCulture(supportedCultures[0])    
                    .AddSupportedCultures(supportedCultures)    
                    .AddSupportedUICultures(supportedCultures) ;     
            });   
            #endregion 
            

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddSession();
            services.Configure<CookiePolicyOptions>(options =>  
            {  
                options.CheckConsentNeeded = context => true;  
                options.MinimumSameSitePolicy = SameSiteMode.None;  
            });  

  
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.AccessDeniedPath = "/Home";
            }); 
            
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddHttpContextAccessor();

            services.AddMvc().AddNToastNotifyNoty(new NotyOptions {
                ProgressBar = true,
                Timeout = 4000,
                Theme = "metroui"
            });
            services.AddDistributedMemoryCache();
            services.AddTransient<IBraintreeService, BraintreeService>();


            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            
            services.AddMvc();  


            

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            DalUser.CreateTable();
            DalDocument.CreateTable();
            DalTheme.CreateTable();
            DalCollection.CreateTable();
            DalAuthor.CreateTable();
            DALStatistics.CreateTable();

            if (env.IsDevelopment())
            {
                app.UseStatusCodePagesWithReExecute("/Home/Error");
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            var supportedCultures = new[] { "en-US", "fr-FR", "ar-TN" };    
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])    
                .AddSupportedCultures(supportedCultures)    
                .AddSupportedUICultures(supportedCultures);    
    
            app.UseRequestLocalization(localizationOptions);
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseDeveloperExceptionPage();
            app.UseNToastNotify();
            app.UseSession();
            app.UseMiddleware(typeof(VisitorCounterMiddleware));


            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                   ForwardedHeaders.XForwardedProto
            });  



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}/"
                    );
                
            });
        }
    }
}
