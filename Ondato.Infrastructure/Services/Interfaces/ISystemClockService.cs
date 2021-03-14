using System;

namespace Ondato.Infrastructure.Services.Interfaces
{
  public interface ISystemClockService
  {
    DateTime UtcNow { get; }
  }
}
