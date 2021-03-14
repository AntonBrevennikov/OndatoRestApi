using AutoMapper;
using Ondato.Business.Dto;
using Ondato.Domain.Models;
using OndatoRestApi.Dto;

namespace Ondato.Business.Mapping
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      CreateMap<StorageItemDto, StorageItem>();
      CreateMap<StorageValueDto, StorageItem>();
      CreateMap<StorageItem, StorageValueDto>();      
    }
  }
}
