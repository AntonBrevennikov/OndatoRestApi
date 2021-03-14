
namespace Ondato.Infrastructure.Services.Interfaces
{
  public interface IConfigurationService
  {
    string ApiKey { get; }

    int LifeTimeSec { get; }

    int CleanupIntervalSec { get; }
  }
}
