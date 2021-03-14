using Ondato.Domain.Models;
using System.Threading.Tasks;

namespace Ondato.Domain.Repositories.Interfaces
{
  public interface IStorageRepository
  {
    StorageItem Get(string key);

    Task<bool> Create(string key, StorageItem item);

    bool Update(string key, StorageItem item);

    bool Delete(string key);
  }
}
