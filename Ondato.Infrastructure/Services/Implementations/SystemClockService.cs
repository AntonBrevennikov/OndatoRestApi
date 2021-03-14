using Ondato.Infrastructure.Services.Interfaces;
using System;

namespace Ondato.Infrastructure.Services.Implementations
{
  public class SystemClockService : ISystemClockService
  {
    public DateTime UtcNow => DateTime.UtcNow;
  }
}
