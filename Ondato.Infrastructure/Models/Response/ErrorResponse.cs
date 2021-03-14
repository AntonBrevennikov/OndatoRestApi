namespace Ondato.Infrastructure.Models.Response
{
  public class ErrorResponse
  {
    public ErrorResponse(ApiError error, int? httpCode)
    {
      Code = httpCode;
      Error = error;     
    }

    public int? Code { get; set; }

    public ApiError Error { get; }
  }
}
