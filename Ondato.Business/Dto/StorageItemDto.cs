using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OndatoRestApi.Dto
{
  public class StorageItemDto
  {
    [Required]
    public string Key { get; set; }

    [Required]
    public List<string> Value { get; set; }

    [Range(0, int.MaxValue)]
    public int LifeTimeSec { get; set; }
  }
}
