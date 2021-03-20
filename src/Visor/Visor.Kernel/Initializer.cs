using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Visor.Abstractions.Entities.Config.Email;
using Visor.Abstractions.User;
using Visor.Abstractions.Utils;
using Visor.Kernel.Registration;
using Visor.Kernel.Utils;

namespace Visor.Kernel
{
    public static class Initializer
    {
        public static void AddKernel(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthMessageSenderOptions>(options => configuration.GetSection("Emails").Bind(options));
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IRegistrationManager, RegistrationManager>();
        }

        public static IApplicationBuilder UseKernel(
           this IApplicationBuilder app)
        {
            
            return app;
        }

    }

}
