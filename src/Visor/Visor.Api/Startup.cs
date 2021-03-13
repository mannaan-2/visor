using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using Visor.Api.Configuration;
using Visor.Data.MySql;
using Visor.Data.MySql.Abstractions;
using Visor.Data.MySql.Tenancy.Pipelines;

namespace Visor.Api
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
            services.AddMySqlIdentityProvider(Configuration, "users");
            //var settingsSection = Configuration.GetSection("AppIdentitySettings");
            //// Inject AppIdentitySettings so that others can use too
            //services.Configure<ApplicationIdentitySettings>(settingsSection);

            //services.AddDbContext<IdentityContext>(options =>
            //    options.UseMySQL(Configuration.GetConnectionString("users"))

            //);
            //services.AddDatabaseDeveloperPageExceptionFilter();

            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<IdentityContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(MyAllowSpecificOrigins);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
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
            app.InitTenantResolution()
              .Then<AttemptResolutionByHost>()
              .Then<AttemptResolutionByQueryString>()
              .Then<AttemptResolutionByReferrer>()
              .Then<AttemptResolutionByCookie>()
              .Then<VerifyTenantResolution, TenantVerificationOptions>(new TenantVerificationOptions(new List<string>()
              {
                   "/.well-known/openid-configuration",
                   "/.well-known/openid-configuration/jwks"
              }));
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
