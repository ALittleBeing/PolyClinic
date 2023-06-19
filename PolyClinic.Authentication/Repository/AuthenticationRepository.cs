using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PolyClinic.Authentication.Models;
using LogEvents = PolyClinic.Common.Logger.LogEvents;

namespace PolyClinic.Authentication.Repository
{
    /// <summary>
    /// Repository for User Authentication
    /// </summary>
    public class AuthenticationRepository
    {
        private readonly AuthenticationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AuthenticationRepository> _logger;

        /// <summary>
        /// Creates Authentication Repository instance with specified logger, DbContext  and UserManager instances
        /// </summary>
        /// <param name="context">AuthenticationDbContext instance</param>
        /// <param name="userManager">UserManager instance</param>
        /// <param name="logger">ILogger instance</param>
        public AuthenticationRepository(AuthenticationDbContext context, UserManager<ApplicationUser> userManager, ILogger<AuthenticationRepository> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Creates new User as an asynchronorous operation
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="email">User Email</param>
        /// <param name="password">Password</param>
        /// <param name="firstName">Password</param>
        /// <param name="lastName">Password</param>
        /// <returns>Empty string if user created successfully. Otherwise, error message</returns>
        public async Task<string> CreateUserAsync(string userName, string email, string password, string firstName, string lastName)
        {
            _logger.LogTrace(LogEvents.TraceMethodEntryMessage(this.GetType().FullName));
            try
            {
                var result = await _userManager.CreateAsync(
                    new ApplicationUser()
                    {
                        UserName = userName,
                        Email = email,
                        FirstName = firstName,
                        LastName = lastName
                    },
                    password: password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.ToList().ConvertAll(err => err.Description);
                    return string.Join(",\n", errors);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.InnerException ?? ex, message: LogEvents.ErrorMessage(ex.InnerException?.Message ?? ex.Message, this.GetType().FullName));
            }
            finally
            {
                _logger.LogTrace(message: LogEvents.TraceMethodExitMessage(this.GetType().FullName));
            }

            return string.Empty;
        }

        /// <summary>
        /// Checks if User exists with given User Name and Password, as an asynchronorous operation
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="password">Password</param>
        /// <returns>User Email if User exists for given username and password. Otherwise, null</returns>
        public async Task<string> LoginAsync(string userName, string password)
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));
            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null) return null;

                var isValid = await _userManager.CheckPasswordAsync(user, password);

                if (isValid)
                {
                    return user.Email;
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.InnerException ?? ex, message: LogEvents.ErrorMessage(ex.InnerException?.Message ?? ex.Message, this.GetType().FullName));
            }
            finally
            {
                _logger.LogTrace(message: LogEvents.TraceMethodExitMessage(this.GetType().FullName));
            }

            return null;
        }

    }
}