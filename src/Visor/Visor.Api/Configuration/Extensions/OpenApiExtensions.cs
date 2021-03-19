using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
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
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://localhost:44382/connect/authorize"),
                            TokenUrl = new Uri("https://localhost:44382/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {

                                {"users.authenticated.scope","Authenticated users" },
                                {"users.anon.scope","Anonymous users" }
                            },
                        },
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://localhost:44382/connect/authorize"),
                            TokenUrl = new Uri("https://localhost:44382/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                {"users.authenticated.scope","Authenticated users" },
                                {"users.anon.scope","Anonymous users" }
                            },
                        }
                    }
                }) ;
                c.OperationFilter<AuthorizeCheckOperationFilter>();
            });
           
            return services;
        }
        public static IApplicationBuilder UseOpenApi(
         this IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Visor Auth API");
                options.RoutePrefix = string.Empty;
                options.OAuthClientId("swagger");
                options.OAuthAppName("Swagger UI client");
                options.OAuthUsePkce();
                
            });
            return app;
        }
    }

    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAuthorize =
              context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
              || context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            if (hasAuthorize)
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    [
                        new OpenApiSecurityScheme {Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "oauth2"}
                        }
                    ] = new[] {"users"}
                }
            };

            }
        }
    }
}
