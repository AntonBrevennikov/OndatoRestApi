using Ondato.Business.Dto;
using OndatoRestApi.Dto;

namespace Ondato.Business.Services.Interfaces
{
  public interface IStorageService
  {
    StorageValueDto Get(string key);

    void Create(StorageItemDto entity); 

    void Update(string key, StorageValueDto entity);

    bool Delete(string key);    
  }
}
