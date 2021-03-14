using Microsoft.AspNetCore.Mvc;
using Ondato.Business.Dto;
using Ondato.Business.Services.Interfaces;
using Ondato.Infrastructure.Request;
using OndatoRestApi.Dto;
using SecuringWebApiUsingApiKey.Attributes;
using System.Net;

namespace Ondato.Api.Controllers
{  
  [ApiKey]
  [ApiController]
  [Route("api/storage")]
  public class StorageController : ControllerBase
  {
    private IStorageService _storageService { get; set; }

    public StorageController(IStorageService storageService)
    {
      _storageService = storageService;
    }

    [HttpGet]
    [Route("{key}")]
    public ActionResult<StorageItemDto> Get(
              [FromRoute] StorageRequest request)
    {
      var item = _storageService.Get(request.Key);
      if (item != null)
      {
        return new OkObjectResult(item);
      }

      return NotFound();
    }

    [HttpPost]
    public ObjectResult Post(
              [FromBody] StorageItemDto item)
    {
      _storageService.Create(item);
      return StatusCode((int)HttpStatusCode.Created, _storageService.Get(item.Key));
    }

    [HttpPut]
    [Route("{key}")]
    public ObjectResult Put(
            [FromRoute] StorageRequest request,
            [FromBody] StorageValueDto item)
    {
      _storageService.Update(request.Key, item);
      return StatusCode((int)HttpStatusCode.Accepted, _storageService.Get(request.Key));
    }

    [HttpDelete]
    [Route("{key}")]
    public IActionResult Delete(
            [FromRoute] StorageRequest request)
    {
      if (_storageService.Delete(request.Key))
      {
        return Ok();
      }

      return NotFound();
    }
  }
}
