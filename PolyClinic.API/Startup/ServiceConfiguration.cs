using PolyClinic.API.Filters;
using PolyClinic.BL.Interface;
using PolyClinic.BL.Services;
using PolyClinic.DAL;
using System.Reflection;

namespace PolyClinic.API.Startup
{
    /// <summary>
    /// Specifies the services to be configured during appication startup
    /// </summary>
    public static class ServiceConfiguration
    {
        /// <summary>
        /// Extension method of IServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <returns>Returns IServiceCollection object</returns>
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            //configure controllers with custom Action filter for logging
            services.AddControllers(Options => Options.Filters.Add(typeof(LogActionFilter)));

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
                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
            //creating new instance of DAL repository to inject into the services
            IPolyClinicRepository polyClinicRepository = new PolyClinicRepository();
            //injecting custom services
            services.AddSingleton<IPatientService>(new PatientService(polyClinicRepository));
            services.AddSingleton<IAppointmentService>(new AppointmentService(polyClinicRepository));
           
            var doctorServiceLogger = services.BuildServiceProvider().GetRequiredService<ILogger<DoctorService>>();
            services.AddSingleton<IDoctorService>(new DoctorService(polyClinicRepository, doctorServiceLogger));
            return services;
        }
    }
}
