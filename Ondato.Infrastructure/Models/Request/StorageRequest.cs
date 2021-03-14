using System.ComponentModel.DataAnnotations;

namespace Ondato.Infrastructure.Request
{
  public class StorageRequest
  {
    [Required]
    public string Key { get; set; }
  }
}
