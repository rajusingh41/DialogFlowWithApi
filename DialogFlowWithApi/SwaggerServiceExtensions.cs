using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Collections.Generic;

namespace DialogFlowWithApi
{
    /// <summary>
    /// SwaggerServiceExtensions
    /// </summary>
    public static class SwaggerServiceExtensions
    {
        /// <summary>
        /// AddSwaggerDocumentation
        /// https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/README-v5.md
        /// https://ppolyzos.com/2017/10/30/add-jwt-bearer-authorization-to-swagger-and-asp-net-core/
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("V1.0", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Dialogflow Bot",
                    Version = "V1.0"
                });

                c.AddSecurityDefinition("Bearer",
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                                },
                                new[] { "readAccess", "writeAccess" }
                            }
                        });
            });
            return services;
        }

        /// <summary>
        /// UseSwaggerDocumentation
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/V1.0/swagger.json", "Dialogflow Bot");
                c.RoutePrefix = string.Empty;
                c.DocExpansion(DocExpansion.None);
            });
            return app;
        }

    }
}
