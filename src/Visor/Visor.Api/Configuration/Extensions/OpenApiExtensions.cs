using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Visor.Api.Configuration.Extensions
{
    public static class OpenApiExtensions
    {
        public static IServiceCollection AddOpenApi(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Visor auth API",
                    Description = "A simple ASP.NET Core Identity Server",
                    TermsOfService = new Uri("https://github.com/mannaan-2/visor"),
                    Contact = new OpenApiContact
                    {
                        Name = "Abdul Manan",
                        Email = string.Empty,
                        Url = new Uri("https://github.com/mannaan-2/visor"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "GPL-3.0",
                        Url = new Uri("https://github.com/mannaan-2/visor"),
                    }
                });
                // Set the comments path for the Swagger JSON and UI.
                //File is only generated if following is added to Proj file under PropertYGroup
                //<GenerateDocumentationFile>true</GenerateDocumentationFile>
                //<NoWarn>$(NoWarn); 1591</NoWarn> //This suppreses the warnings in solution
                   var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
           
            return services;
        }
        public static IApplicationBuilder UseOpenApi(
         this IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Visor Auth API");
                c.RoutePrefix = string.Empty;
            });
            return app;
        }
    }
}
