using Microsoft.AspNetCore.Mvc.Filters;

namespace PolyClinic.API.Filters
{
    /// <summary>
    /// Custom Action Filter for Logs on Action excecution
    /// </summary>
    public class LogActionFilter:IActionFilter
    {
        private readonly ILogger<LogActionFilter> _logger;

        /// <summary>
        /// Constructor of Custom Log Action Filter
        /// </summary>
        /// <param name="logger">Injected Logger instance</param>
        public LogActionFilter(ILogger<LogActionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Implementation of OnActionExecuting method
        /// </summary>
        /// <param name="context">Action Executing Context</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogTrace(
                message: "[OnActionExecuting] \tRun endpoint: \t/{controller}/{action}/", 
                context.RouteData.Values["controller"], context.RouteData.Values["action"]);
        }

        /// <summary>
        /// Implementation of OnActionExecuted method
        /// </summary>
        /// <param name="context">Action Executed Context</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogTrace(
                message: "[OnActionExecuted] \tExiting from endpoint: \t/{controller}/{action}/",
                context.RouteData.Values["controller"], context.RouteData.Values["action"]);
        }
    }
}
