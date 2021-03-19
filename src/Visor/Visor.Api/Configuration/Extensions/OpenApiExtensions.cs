using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Visor.Abstractions.Entities.Config.OpenApi;

namespace Visor.Api.Configuration.Extensions
{
    public static class OpenApiExtensions
    {
        public static IServiceCollection AddOpenApi(this IServiceCollection services, IConfiguration configuration)
        {
            var settingsSection = configuration.GetSection("OpenApi");
            // Inject AppIdentitySettings so that others can use too
            services.Configure<Settings>(settingsSection);

            var openApiSettings = settingsSection.Get<Settings>();
            services.AddSwaggerGen(c =>
            {
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.SwaggerDoc(openApiSettings.Version, new OpenApiInfo
                {
                    Version = openApiSettings.Version,
                    Title = openApiSettings.Title,
                    Description = openApiSettings.Description,
                    TermsOfService = new Uri(openApiSettings.TermsOfServiceUrl),
                    Contact = new OpenApiContact
                    {
                        Name = openApiSettings.ContactName,
                        Email = openApiSettings.ContactEmail,
                        Url = new Uri(openApiSettings.ContactUrl),
                    },
                    License = new OpenApiLicense
                    {
                        Name = openApiSettings.License,
                        Url = new Uri(openApiSettings.LicenseUrl),
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
                            AuthorizationUrl = new Uri(openApiSettings.AuthorizationUrl),
                            TokenUrl = new Uri(openApiSettings.TokenUrl),
                            Scopes = openApiSettings.Scopes,
                        },
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(openApiSettings.AuthorizationUrl),
                            TokenUrl = new Uri(openApiSettings.TokenUrl),
                            Scopes = openApiSettings.Scopes,
                        }
                    }
                });
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
