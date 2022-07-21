
using FinanceAnalytic.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Serialization;

namespace FinanceAnalytic
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {

            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllersWithViews();
            services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("AppDbContext")));




            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie(options =>
                            {
                                options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                            });

            //services.AddMvc();

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("admin", policy => policy.RequireRole("admin"));
            //});


            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("admin", policy => policy.RequireClaim("admin"));
            //    options.AddPolicy("User", policy => policy.RequireClaim("User", "admin"));
            //});

            //services.AddAuthorization(options =>
            //options.AddPolicy("UserRole",
            // policy => policy.RequireClaim("User", "admin")));

            //services.AddAuthorization(config =>
            //{
            //    var userAuthPolicyBuilder = new AuthorizationPolicyBuilder();
            //    config.DefaultPolicy = userAuthPolicyBuilder
            //                        .RequireAuthenticatedUser()
            //                        .RequireClaim(ClaimTypes.Role)
            //                        .Build();
            //});

            //services.AddAuthorization();


            services.AddControllersWithViews();
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
            });
            services.AddHostedService<CurrencyBackgroundService>();
            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var defaultDateCulture = "ru-RU";
            var ci = new CultureInfo(defaultDateCulture);
            ci.NumberFormat.NumberDecimalSeparator = ".";
            ci.NumberFormat.CurrencyDecimalSeparator = ".";

            // Configure the Localization middleware
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(ci),
                SupportedCultures = new List<CultureInfo> { ci, },
                SupportedUICultures = new List<CultureInfo> { ci, }
            });

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
            app.UseBrowserLink();
            app.UseAuthentication();
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
