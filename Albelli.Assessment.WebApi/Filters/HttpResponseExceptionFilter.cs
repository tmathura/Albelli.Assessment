using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Albelli.Assessment.WebApi.Filters
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(HttpResponseExceptionFilter));

        public int Order => int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is HttpResponseException httpResponseException)
            {
                context.Result = new ObjectResult(httpResponseException.Value)
                {
                    StatusCode = httpResponseException.StatusCode
                };

                context.ExceptionHandled = true;

                _logger.Error($"Status code {httpResponseException.StatusCode}, {httpResponseException.Value} - {httpResponseException.Message} - {httpResponseException.StackTrace}");
            }
        }
    }
}
