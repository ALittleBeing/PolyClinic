using Microsoft.OpenApi.Models;
using System.Reflection;

namespace PolyClinic.API.Startup
{
    /// <summary>
    /// Specifies Swagger configurations
    /// </summary>
    public static class SwaggerConfiguration
    {

        /// <summary>
        /// Extension method of IServiceCollection for configuring services
        /// </summary>
        /// <param name="services"></param>
        /// <returns>Returns IServiceCollection object</returns>
        public static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "PolyClinic Api",
                    Description = "An ASP.NET CORE Web API for PolyClinic",
                    TermsOfService = new Uri("https://learn.microsoft.com/"),
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "API Developer",
                        Email = "api@developer.com",
                        Url = new Uri("https://learn.microsoft.com/")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense
                    {
                        Name = "Development License",
                        Url = new Uri("https://learn.microsoft.com/")
                    },

                });
                // Configure Swagger Authorization
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });


                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
            return services;
        }
        /// <summary>
        /// Extension method of WebApplication to use swagger with custom configurations
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static WebApplication UseSwaggerService(this WebApplication app)
        {
            /*if (app.Environment.IsDevelopment())
            {
            }*/
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
