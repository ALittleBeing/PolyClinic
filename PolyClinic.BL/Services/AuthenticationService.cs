using Microsoft.Extensions.Logging;
using PolyClinic.BL.Interface;
using PolyClinic.Authentication.Repository;
using PolyClinic.Common.Models;

namespace PolyClinic.BL.Services
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly AuthenticationRepository _authRepository;
        private readonly JwtRepository _jwtRepository;
        private readonly ILogger<AuthenticationService> _logger;

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
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", System.Reflection.MethodInfo.GetCurrentMethod().ToString());

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
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: {name}", ex.Message, this.GetType().FullName);
                //throw;

            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \t\tMethod: \t{name}", System.Reflection.MethodInfo.GetCurrentMethod().Name);
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
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType().FullName);
            string userEmail = null;
            try
            {
                userEmail = await _authRepository.LoginAsync(userName: request.UserName, password: request.Password);

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: {name}", ex.Message, this.GetType().FullName);

            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \t\tMethod: \t{name}", this.GetType().FullName);
            }

            return userEmail;
        }

        public AuthenticationResponse CreateToken(string userName, string userEmail)
        {
            _logger.LogTrace(message: "[OnMethodExecuting] \tMethod: \t{name}", this.GetType().FullName);
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
                _logger.LogError(ex, message: "Exception Message: {msg}.\n Occurred on Method: {name}", ex.Message, this.GetType().FullName);

            }
            finally
            {
                _logger.LogTrace(message: "[OnMethodExecuted] \t\tMethod: \t{name}", this.GetType().FullName);
            }

            return response;
        }

    }
}