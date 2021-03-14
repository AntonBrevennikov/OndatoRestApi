using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using AutoMapper;
using Ondato.Infrastructure.Services.Interfaces;
using Ondato.Infrastructure.Services.Implementations;
using Ondato.Business.Services.Interfaces;
using Ondato.Business.Services.Implementations;
using Ondato.Domain.Repositories.Interfaces;
using Ondato.Domain.Repositories.Implementations;
using Ondato.Api.Filters;
using Ondato.Business.Mapping;
using Ondato.Infrastructure.Factories;

namespace Ondato.Api
{
  public static class ApplicationServices
  {
    public static void AddApplicationServices(this IServiceCollection services)
    {
      services.AddSingleton<ISystemClockService, SystemClockService>();
      services.AddSingleton<IConfigurationService, ConfigurationService>();

      services.AddScoped<IStorageService, StorageService>();      
    }

    public static void AddRepositories(this IServiceCollection services)
    {
      services.AddSingleton<IStorageRepository, StorageRepository>();
    }

    public static void AddSwaggerGeneration(this IServiceCollection services, string title, string version)
    {
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc(version, new OpenApiInfo { Title = title, Version = version });
        c.OperationFilter<LowercasePropertyFilter>();
      });
    }

    public static void AddAutoMapper(this IServiceCollection services)
    {
      services.AddSingleton(new MapperConfiguration(c => c.AddProfile(new MappingProfile())).CreateMapper());
    }

    public static void AddValidationErrorResponse(this IServiceCollection services)
    {
      services.Configure<ApiBehaviorOptions>(opt =>
      {
        opt.InvalidModelStateResponseFactory = ModelStateResponseFactory.CreateInvalidModelStateResponseFactory();
      });
    }
  }
}
