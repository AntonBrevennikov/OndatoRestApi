using Microsoft.Extensions.Logging;
using Ondato.Domain.Models;
using Ondato.Domain.Repositories.Interfaces;
using Ondato.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Ondato.Domain.Repositories.Implementations
{
  public class StorageRepository : IStorageRepository
  {
    private readonly ConcurrentDictionary<string, StorageItem> _dictStorage = new ConcurrentDictionary<string, StorageItem>();
    
    private readonly IConfigurationService _configurationService;
    private readonly ISystemClockService _systemClockService;

    private readonly ILogger _logger;

    public StorageRepository(
      IConfigurationService configurationService, 
      ISystemClockService systemClockService,
      ILogger<StorageRepository> logger)
    {
      _configurationService = configurationService;
      _systemClockService = systemClockService;
      _logger = logger;

      var clenupTimer = new Timer();
      clenupTimer.Elapsed += new ElapsedEventHandler(OnTimerHanler);
      clenupTimer.Interval = TimeSpan.FromSeconds(_configurationService.CleanupIntervalSec).TotalMilliseconds;
      clenupTimer.Enabled = true;
    }

    public StorageItem Get(string key)
    {
      StorageItem item;
      if (_dictStorage.TryGetValue(key, out item))
      {
        return ResetItem(key, item).Result;
      }

      return null;
    }

    public Task<bool> Create(string key, StorageItem item)
    {
      return Task<bool>.Factory.StartNew(() =>
      {
        if (item.LifeTimeSec == 0)
        {
          item.LifeTimeSec = _configurationService.LifeTimeSec;
        }

        return _dictStorage.TryAdd(key, item);
      });
    }

    public bool Update(string key, StorageItem item)
    {
      StorageItem currItem;
      return _dictStorage.TryGetValue(key, out currItem) && _dictStorage.TryUpdate(key, item, currItem);
    }

    public bool Delete(string key)
    {
      return _dictStorage.TryRemove(key, out _);
    }    

    private void OnTimerHanler(object source, ElapsedEventArgs e)
    {
      var expiredKeys = _dictStorage.Where((data) => IsItemExpired(data.Value)).Select(data => data.Key);
      foreach (var key in expiredKeys)
      {       
        if (_dictStorage.TryRemove(key, out _))
        {
          _logger.LogDebug($"Expired item {key} removed successfully");
        }
      }
    } 
    
    private bool IsItemExpired(StorageItem item)
    {
      return (_systemClockService.UtcNow - item.UpdateDate).TotalSeconds >= item.LifeTimeSec;
    }

    private Task<StorageItem> ResetItem(string key, StorageItem item)
    {
      return Task<StorageItem>.Factory.StartNew(() =>
      {
        item.UpdateDate = _systemClockService.UtcNow;
        if (Update(key, item))
        {
          _logger.LogDebug($"Performed reset operation for item {key}");

          return item;
        }

        StorageItem currItem;
        return _dictStorage.TryGetValue(key, out currItem) ? currItem : null;
      });
    }
  }
}
