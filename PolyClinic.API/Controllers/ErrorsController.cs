using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace PolyClinic.API.Controllers
{
    /// <summary>
    /// Error Controller class for ErrorHandler middleware
    /// </summary>
    [ApiExplorerSettings(IgnoreApi = true)]
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
        /// Handles Exceptions from Controllers globally
        /// </summary>
        /// <returns></returns>
        [Route("/error")]
        public IActionResult HandleError()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>().Error;
            var message = "[Exception]\t\tMethod:\t" + nameof(exception.TargetSite.GetType)
                            + "\n[Message]\t" + exception.Message;
            _logger.LogError(exception: exception, message: message);

            return Problem(
                detail: "Some error occurred. Please try again later.",
                title: "Bad Request",
                statusCode: 400);
        }
    }
}
