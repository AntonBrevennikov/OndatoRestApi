using AutoMapper;
using Ondato.Business.Dto;
using Ondato.Business.Services.Interfaces;
using Ondato.Domain.Models;
using Ondato.Domain.Repositories.Interfaces;
using Ondato.Infrastructure.Exceptions;
using Ondato.Infrastructure.Services.Interfaces;
using OndatoRestApi.Dto;
using System;

namespace Ondato.Business.Services.Implementations
{
  public class StorageService : IStorageService {

    private readonly IMapper _mapper;
    private readonly IStorageRepository _storageRepository;
    private readonly ISystemClockService _systemClockService;

    public StorageService(
            IMapper mapper,           
            IStorageRepository storageRepository,
            ISystemClockService systemClockService)
    {
      _mapper = mapper;
      _storageRepository = storageRepository;
      _systemClockService = systemClockService;
    }

    public StorageValueDto Get(string key)
    {
      return _mapper.Map<StorageValueDto>(_storageRepository.Get(key));
    }

    public void Create(StorageItemDto item)
    {
      try
      {
        var storageItem = _mapper.Map<StorageItem>(item);
        storageItem.UpdateDate = _systemClockService.UtcNow;

        if (!_storageRepository.Create(item.Key, storageItem).Result)
        {
          _storageRepository.Update(item.Key, storageItem);
        }
      }
      catch (Exception ex)
      { 
        throw new ServiceException(ex.Message, "Create storage item");
      }
    }

    public void Update(string key, StorageValueDto item)
    {
      try
      {
        var storageItem = _mapper.Map<StorageItem>(item);
        storageItem.UpdateDate = _systemClockService.UtcNow;

        if (!_storageRepository.Update(key, storageItem))
        {
          _storageRepository.Create(key, storageItem).Wait();
        }
      }
      catch (Exception ex)
      {
        throw new ServiceException(ex.Message, "Update storage item");
      }
    }

    public bool Delete(string key)
    {      
      return _storageRepository.Delete(key);
    }    
  }
}
