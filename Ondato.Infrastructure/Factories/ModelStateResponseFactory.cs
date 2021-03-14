using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Ondato.Infrastructure.Extensions;
using Ondato.Infrastructure.Models.Response;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Ondato.Infrastructure.Factories
{
  public static class ModelStateResponseFactory
  {
    public static Func<ActionContext, IActionResult> CreateInvalidModelStateResponseFactory()
    {
      return context =>
      {
        if (context.HttpContext.Request.Method != "GET" && context.HttpContext.Request.Method != "DELETE" && IsJsonRequest(context) && !IsValidJsonInBody(context))
        {
          return new UnprocessableEntityObjectResult(new ErrorResponse(new ApiError("json", "Invalid request"), StatusCodes.Status422UnprocessableEntity));
        }

        return CreateValidationErrorResponse(context, StatusCodes.Status400BadRequest);
      };
    }

    private static bool IsJsonRequest(ActionContext context)
    {
      return context.HttpContext.Request.ContentType.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Any(t => t.StartsWith("application/json", StringComparison.OrdinalIgnoreCase));
    }

    private static IActionResult CreateValidationErrorResponse(ActionContext context, int statusCode)
    {
      var error = context.ModelState.Where(s => s.Value.Errors.Count > 0).Select(e =>
      {
        var key = e.Key.ToLowerCaseKey();
        var nestedFieldName = key.Split('.').Last();

        return new ApiError(key, $"Invalid or missing {nestedFieldName}");
      }).FirstOrDefault();

      return new BadRequestObjectResult(new ErrorResponse(error, statusCode));
    }

    private static bool IsValidJsonInBody(ActionContext context)
    {
      try
      {
        // Body has already been read by MVC so needs to be reset back to the beginning
        context.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);

        string body;
        using (StreamReader reader = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8))
        {
          body = reader.ReadToEnd();
        }

        return JsonConvert.DeserializeObject(body) != null;
      }
      catch
      {        
      }

      return false;
    }
  }
}
