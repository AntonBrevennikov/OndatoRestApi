using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Ondato.Infrastructure.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SecuringWebApiUsingApiKey.Attributes
{
  [AttributeUsage(validOn: AttributeTargets.Class)]
  public class ApiKeyAttribute : Attribute, IAsyncActionFilter
  {
    private const string APIKEYNAME = "ApiKey";
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      if (!context.HttpContext.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
      {
        context.Result = new ContentResult()
        {
          StatusCode = 401,
          Content = "Authorization key not provided"
        };
        return;
      }

      var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfigurationService>();
      var apiKey = appSettings.ApiKey;

      if (!apiKey.Equals(extractedApiKey))
      {
        context.Result = new ContentResult()
        {
          StatusCode = 401,
          Content = "Unauthorized client"
        };
        return;
      }

      await next();
    }
  }
}