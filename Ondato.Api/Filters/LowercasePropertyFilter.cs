using Microsoft.OpenApi.Models;
using Ondato.Infrastructure.Extensions;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Ondato.Api.Filters
{
  public class LowercasePropertyFilter : IOperationFilter
  {
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
      if (operation.Parameters != null)
      {
        foreach (var param in operation.Parameters)
        {
          param.Name = param.Name.ToLowerCaseKey();
        }
      }
    }
  } 
}
