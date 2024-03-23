using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PixelService.Api.Application.Interfaces;
using PixelService.Api.Infrastructure.Blob;
using PixelService.Api.Infrastructure.Interfaces;
using PixelService.Api.WebApi.Services;
using PixelService.Api.WebApi.Settings;

namespace PixelService.Api.WebApi.Extensions;

public static class DependencyInjectionExtensions
{
    public static void AddSettings(this IServiceCollection services)
    {
        services
            .AddOptions<AppSettings>()
            .Configure<IConfiguration>((settings, config) => config.Bind("AppSettings", settings));
    }
    
    public static void AddServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IUserDataCollector, HttpUserDataCollector>();
        services.AddScoped<IBlobContentProvider, MemoryBlobContentProvider>();
    }
}