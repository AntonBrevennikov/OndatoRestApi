using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ondato.Business.Dto
{
  public class StorageValueDto
  {
    [Required]
    public List<string> Value { get; set; }
  }
}
