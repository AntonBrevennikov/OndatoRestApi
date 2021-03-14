using Microsoft.Extensions.Configuration;
using Ondato.Infrastructure.Extensions;
using Ondato.Infrastructure.Services.Interfaces;

namespace Ondato.Infrastructure.Services.Implementations
{
  public class ConfigurationService : IConfigurationService
  {
    const int LIFETIMESEC = 60;

    const int CLEANUPSEC = 20;

    private readonly IConfiguration _configuration;

    public ConfigurationService(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public string ApiKey => _configuration["AppSettings:ApiKey"];

    public int LifeTimeSec => _configuration["AppSettings:LifeTimeSec"].ConvertToInt(LIFETIMESEC);

    public int CleanupIntervalSec => _configuration["AppSettings:CleanupSec"].ConvertToInt(CLEANUPSEC);
  }
}
