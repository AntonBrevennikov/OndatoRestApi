using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Ondato.Infrastructure.Exceptions;
using Ondato.Infrastructure.Models.Response;

namespace Ondato.Api.Filters
{
  public class GlobalExceptionFilter : IExceptionFilter
  {
    private readonly ILogger _logger; 

    public GlobalExceptionFilter(
        ILogger<GlobalExceptionFilter> logger)
    {
      _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
      var methodName = (context.ActionDescriptor is ControllerActionDescriptor)
          ? ((ControllerActionDescriptor)context.ActionDescriptor).ActionName
          : context.Exception.TargetSite.Name;

      var pathValue = context.HttpContext.Request.Path.Value;

      var logMessage = $"Exception {pathValue} {methodName}";
      _logger.LogError(context.Exception, logMessage);

      ErrorResponse errorResponse;
      int? httpStatusCode = StatusCodes.Status500InternalServerError;

      if (context.Exception is ServiceException serviceException)
      {
        errorResponse = CreateErrorResponse(serviceException, httpStatusCode);
      }     
      else
      {
        errorResponse = CreateErrorResponse(new ApiError(string.Empty, context.Exception.Message), httpStatusCode);
      }

      context.Result = new ObjectResult(errorResponse)
      {
        StatusCode = httpStatusCode
      };
    }

    private ErrorResponse CreateErrorResponse(ApiError error, int? httpCode)
    {
      return new ErrorResponse(error, httpCode);
    }

    private ErrorResponse CreateErrorResponse(ServiceException exception, int? httpCode)
    {
      return new ErrorResponse(new ApiError(exception.ServiceName, exception.Message), httpCode);
    }   
  }
}
