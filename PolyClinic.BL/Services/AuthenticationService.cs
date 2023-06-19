using Microsoft.Extensions.Logging;
using PolyClinic.Authentication.Repository;
using PolyClinic.BL.Interface;
using PolyClinic.Common.Models;
using LogEvents = PolyClinic.Common.Logger.LogEvents;

namespace PolyClinic.BL.Services
{
    /// <summary>
    /// Business Logic service for User Authentication and Authorization
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationRepository _authRepository;
        private readonly JwtRepository _jwtRepository;
        private readonly ILogger<AuthenticationService> _logger;

        /// <summary>
        /// Creates Authentication service instance with specified repositories and logger instances
        /// </summary>
        /// <param name="authenticationRepository">Authentication Repository instance</param>
        /// <param name="jwtRepository">Jwt Repository instance</param>
        /// <param name="logger">ILogger instance</param>
        public AuthenticationService(AuthenticationRepository authenticationRepository, JwtRepository jwtRepository, ILogger<AuthenticationService> logger)
        {
            _authRepository = authenticationRepository;
            _jwtRepository = jwtRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates new User with given User Name, Email and Password, as an asynchronorous operation
        /// </summary>
        /// <param name="user">User object instance</param>
        /// <returns>Empty string if user created successfully. Otherwise, error message</returns>
        public async Task<string> CreateUserAsync(User user)
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));

            string result = null;
            try
            {
                result = await _authRepository.CreateUserAsync(
                    userName: user.UserName,
                    email: user.Email,
                    password: user.Password,
                    firstName: user.FirstName,
                    lastName: user.LastName);
            }
            catch (System.Exception ex)
            {
                result = ex.Message;
                _logger.LogError(ex, message: LogEvents.ErrorMessage(ex.Message, this.GetType().FullName));
            }
            finally
            {
                _logger.LogTrace(message: LogEvents.TraceMethodExitMessage(this.GetType().FullName));
            }

            return result;
        }

        /// <summary>
        /// Checks if User exists with given User Name and Password, as an asynchronorous operation
        /// </summary>
        /// <param name="request">Authentication Request with UserName and Password</param>
        /// <returns>User Email if User exists for given username and password. Otherwise, null</returns>
        public async Task<string> ValidateLoginAsync(AuthenticationRequest request)
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));

            string userEmail = null;
            try
            {
                userEmail = await _authRepository.LoginAsync(userName: request.UserName, password: request.Password);

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, message: LogEvents.ErrorMessage(ex.Message, this.GetType().FullName));

            }
            finally
            {
                _logger.LogTrace(message: LogEvents.TraceMethodExitMessage(this.GetType().FullName));
            }

            return userEmail;
        }

        /// <summary>
        /// Creates Authentication Token with given User Name and Email
        /// </summary>
        /// <param name="userName">UserName</param>
        /// <param name="userEmail">User Email Address</param>
        /// <returns>AuthenticationResponse with Token and Expiration date/time. Otherwise, null</returns>
        public AuthenticationResponse CreateToken(string userName, string userEmail)
        {
            _logger.LogTrace(message: LogEvents.TraceMethodEntryMessage(this.GetType().FullName));

            AuthenticationResponse response = null;
            try
            {
                var token = _jwtRepository.CreateToken(userName, userEmail, out DateTime expiration);
                response = new AuthenticationResponse()
                {
                    Token = token,
                    Expiration = expiration
                };

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, message: LogEvents.ErrorMessage(ex.Message, this.GetType().FullName));

            }
            finally
            {
                _logger.LogTrace(message: LogEvents.TraceMethodExitMessage(this.GetType().FullName));
            }

            return response;
        }

    }
}