using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PolyClinic.BL.Interface;
using PolyClinic.Common.Models;

namespace PolyClinic.API.Controllers
{
    /// <summary>
    /// User Controller class
    /// </summary>
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<UserController> _logger;
        /// <summary>
        /// Creates User controller instance with injected authentication service
        /// </summary>
        /// <param name="service">BL service injected through Dependency Injection</param>
        /// <param name="logger"></param>
        public UserController(IAuthenticationService service, ILogger<UserController> logger)
        {
            _authenticationService = service;
            _logger = logger;
        }

        /// <summary>
        /// Adds a new User
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns username upon successful action</returns>
        /// <response code="201">If new User is added successfully</response>
        /// <response code="400">If any error occurs </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddUser(User user)
        {
            var result = await _authenticationService.CreateUserAsync(user);
            if (string.IsNullOrEmpty(result))
            {
                _logger.LogInformation("Added new User with username [{username}] successfully", user.UserName);
                return Created("", "User created successfully with UserName: " + user.UserName);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// Authenticates User and generates authentication Token
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Returns authentication Token with expiration date/time</returns>
        /// <response code="200">If user is authenticated successfully</response>
        /// <response code="400">If credentials are invalid or if any error occurs </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(AuthenticationRequest request)
        {
            var userEmail = await _authenticationService.ValidateLoginAsync(request);

            if (string.IsNullOrEmpty(userEmail))
            {
                _logger.LogInformation("Invalid Credentials provided. UserName: {username}", request.UserName);
                return BadRequest("Invalid Credentials");
            }

            var response = _authenticationService.CreateToken(request.UserName, userEmail);
            _logger.LogInformation("Generated JWT Bearer token for username [{username}] successfully", request.UserName);
            return Ok(response);

        }

    }
}