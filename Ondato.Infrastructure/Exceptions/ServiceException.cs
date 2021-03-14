using System;

namespace Ondato.Infrastructure.Exceptions
{
  public class ServiceException : Exception
  {
    public ServiceException(string message, string serviceName)
            : base(message)
    {
      ServiceName = serviceName;
    }

    public string ServiceName { get; }
  }
}
