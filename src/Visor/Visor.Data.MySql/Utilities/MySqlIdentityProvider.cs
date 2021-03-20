﻿using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Visor.Data.MySql.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Visor.Data.MySql.Identity.Entities;
using Visor.Data.MySql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Visor.Data.MySql.Constants;
using IdentityServer4;
using Visor.Abstractions.Entities.Config.Identity;
using Visor.Tenancy;
using Visor.Abstractions.Repositories;
using Visor.Data.MySql.Repositories;

namespace Visor.Data.MySql.Utilities
{
    public static class MySqlIdentityProvider
    {
        public static IServiceCollection AddTenantedMySqlIdentityProvider(this IServiceCollection services,
          IConfiguration configuration, string connectionString)
        {
            DesignTimeIdDbContextFactory.SetConnectionString(configuration.GetConnectionString(connectionString));
            services.AddMySqlIdentity(configuration, connectionString);
            services.AddMySqlTenancy();
            services.MigrateDatabase();

            services.AddIdentityServer(configuration);
            services.ConfigureCookies();

            services.AddHttpContextAccessor();
            services.InitializeDefaultTenant(connectionString);

            services.AddTransient<IUserRepository, UserRepository>();
            return services;
        }
        public static IApplicationBuilder UseTenantedMySqlIdentityProvider(
          this IApplicationBuilder app)
        {
            app.UseTenants();
            return app;
        }
        private static IServiceCollection AddMySqlIdentity(this IServiceCollection services,
         IConfiguration configuration, string connectionString)
        {
            var settingsSection = configuration.GetSection("AppIdentitySettings");
            // Inject AppIdentitySettings so that others can use too
            services.Configure<ApplicationIdentitySettings>(settingsSection);

            var identitySettings = settingsSection.Get<ApplicationIdentitySettings>();

            services.AddDbContext<IdentityContext>(options =>
                options.UseMySQL(configuration.GetConnectionString(connectionString))

            );

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                // User settings
                options.User.RequireUniqueEmail = identitySettings.User.RequireUniqueEmail;

                // Password settings
                options.Password.RequireDigit = identitySettings.Password.RequireDigit;
                options.Password.RequiredLength = identitySettings.Password.RequiredLength;
                options.Password.RequireLowercase = identitySettings.Password.RequireLowercase;
                options.Password.RequireNonAlphanumeric = identitySettings.Password.RequireNonAlphanumeric;
                options.Password.RequireUppercase = identitySettings.Password.RequireUppercase;

                // Lockout settings
                options.Lockout.AllowedForNewUsers = identitySettings.Lockout.AllowedForNewUsers;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(identitySettings.Lockout.DefaultLockoutTimeSpanInMins);
                options.Lockout.MaxFailedAccessAttempts = identitySettings.Lockout.MaxFailedAccessAttempts;

                // Signin settings
                options.SignIn.RequireConfirmedEmail = identitySettings.SignIn.RequireConfirmedEmail;
                options.SignIn.RequireConfirmedAccount = identitySettings.SignIn.RequireConfirmedAccount;

            })
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();


            return services;
        }
        private static IServiceCollection MigrateDatabase(this IServiceCollection services)
        {
           
            var serviceProvider = services.BuildServiceProvider();
            var identityContext = serviceProvider.GetService<IdentityContext>();
            // identityContext.Database.EnsureCreated();
            if (identityContext.Database.GetPendingMigrations().Any())
            {
                identityContext.Database.Migrate();
            }

            return services;
        }
        public static IServiceCollection AddIdentityServer(this IServiceCollection services,
          IConfiguration configuration)
        {
          
            //// Adds IdentityServer
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(IdentityServerConfig.Ids)
                .AddInMemoryApiScopes(configuration.GetSection("IdentityServer:ApiScopes"))
                .AddInMemoryApiResources(configuration.GetSection("IdentityServer:ApiResources"))
                //.AddInMemoryClients(IdentityServerConfig.swaggerClient)
                .AddInMemoryClients(configuration.GetSection("IdentityServer:Clients"))
                .AddAspNetIdentity<ApplicationUser>();
            //.AddProfileService<IdentityProfileService>();
            /**DONT**/
            //IdentityModelEventSource.ShowPII = true;

            return services;
        }
        public static IServiceCollection ConfigureCookies(this IServiceCollection services)
        {
            
            services.Configure<CookieAuthenticationOptions>(IdentityServerConstants.DefaultCookieAuthenticationScheme, options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.IsEssential = true;
            });
            services.ConfigureApplicationCookie(options =>
            {
                //options.LoginPath = $"/Identity/Account/Login";
                //options.LogoutPath = $"/Identity/Account/Logout";
                //options.AccessDeniedPath = $"/Identity/Account/AccessDenied";

                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.Cookie.SameSite = SameSiteMode.None;
                options.SlidingExpiration = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
            return services;
        }
    }
}
