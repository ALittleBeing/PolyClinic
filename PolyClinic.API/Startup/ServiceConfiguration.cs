using Microsoft.AspNetCore.Identity;
using PolyClinic.API.Filters;
using PolyClinic.Authentication;
using PolyClinic.Authentication.Models;
using PolyClinic.Authentication.Repository;
using PolyClinic.BL.Interface;
using PolyClinic.BL.Services;
using PolyClinic.DAL.Repository;

namespace PolyClinic.API.Startup
{
    /// <summary>
    /// Specifies the services to be configured during appication startup
    /// </summary>
    public static class ServiceConfiguration
    {
        /// <summary>
        /// Extension method of IServiceCollection for configuring services
        /// </summary>
        /// <param name="services"></param>
        /// <returns>Returns IServiceCollection object</returns>
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            // configure controllers with custom Action filter for logging
            services.AddControllers(Options => Options.Filters.Add(typeof(LogActionFilter)));
            // Adding Swagger with custom configuration
            services.AddSwaggerService();
            // Adding Identity for Authentication
            services.AddDbContext<AuthenticationDbContext>();
            services.AddIdentityCore<ApplicationUser>(options => options.User.RequireUniqueEmail = true)
                    .AddEntityFrameworkStores<AuthenticationDbContext>();

            // Adding custom business logic services
            services.AddScoped<IAppointmentService>(provider =>
                new AppointmentService(
                    new AppointmentRepository(provider.GetRequiredService<ILogger<AppointmentRepository>>()),
                    provider.GetRequiredService<ILogger<AppointmentService>>()));

            services.AddScoped<IPatientService>(provider =>
                new PatientService(
                    new PatientRepository(provider.GetRequiredService<ILogger<PatientRepository>>()),
                    provider.GetRequiredService<ILogger<PatientService>>()));

            services.AddScoped<IDoctorService>(provider =>
                new DoctorService(
                    new DoctorRepository(provider.GetRequiredService<ILogger<DoctorRepository>>()),
                    provider.GetRequiredService<ILogger<DoctorService>>()));

            // Adding custom Authentication service
            services.AddScoped<IAuthenticationService>(provider =>
                new AuthenticationService(
                    new AuthenticationRepository(provider.GetRequiredService<AuthenticationDbContext>(),
                        provider.GetRequiredService<UserManager<ApplicationUser>>(),
                        provider.GetRequiredService<ILogger<AuthenticationRepository>>()),
                    new JwtRepository(),
                    provider.GetRequiredService<ILogger<AuthenticationService>>()));


            return services;
        }
    }
}
