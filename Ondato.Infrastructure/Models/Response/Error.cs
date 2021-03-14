using System;

namespace Ondato.Infrastructure.Models.Response
{
  public class ApiError
  {
    public ApiError(string name, string message)
    {
      Name = name ?? throw new ArgumentNullException(nameof(name));
      Message = message ?? throw new ArgumentNullException(nameof(message));
    }

    public string Name { get; }

    public string Message { get; }
  }
}
