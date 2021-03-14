using System;
using System.Collections.Generic;

namespace Ondato.Domain.Models
{
  public class StorageItem
  {
    public List<string> Value { get; set; }

    public DateTime UpdateDate { get; set; }

    public int LifeTimeSec { get; set; }   
  }
}
