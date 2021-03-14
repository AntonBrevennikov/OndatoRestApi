using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ondato.Api.Filters;

namespace Ondato.Api
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddApplicationServices();
      services.AddRepositories();
      services.AddAutoMapper();

      services.AddCors();
      services.AddControllers(options => 
      {
        options.Filters.Add(typeof(GlobalExceptionFilter));
      });

      services.AddValidationErrorResponse();
      services.AddSwaggerGeneration("Ondato Rest Api Service", "v1");
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      { 
        app.UseHsts();
      }
      
      app.UseHttpsRedirection();
      app.UseRouting();

      app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

      app.UseAuthorization();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });

      app.UseSwagger();
      app.UseSwaggerUI(options =>
      {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Ondato Rest Api");
      }); 
    }
  }
}
