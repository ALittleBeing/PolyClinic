using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PolyClinic.API.Controllers
{
    /// <summary>
    /// Error Controller class for ErrorHandler middleware
    /// </summary>
    [ApiExplorerSettings(IgnoreApi =true)]
    public class ErrorsController : ControllerBase
    {
        private readonly ILogger<ErrorsController> _logger;

        /// <summary>
        /// Constructor of Error controller
        /// </summary>
        /// <param name="logger">Injected ILogger instance</param>
        public ErrorsController(ILogger<ErrorsController> logger)
        {
            _logger = logger;
        }
        
        /// <summary>
        /// Method to handle Exceptions globally
        /// </summary>
        /// <returns></returns>
        [Route("/error")]
        public IActionResult HandleError() 
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>().Error;
            _logger.LogError(exception: exception, message: nameof(exception.TargetSite.GetType));

            return Problem(
                detail: "Some error occurred. Please try again later.",
                title: "Bad Request",
                statusCode: 400);
        }
    }
}
