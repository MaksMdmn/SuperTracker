using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StorageService.Api.Domain.Visitors;
using StorageService.Api.Infrastructure.File.Repositories;
using StorageService.Api.MessageApi.Settings;

namespace StorageService.Api.MessageApi.Extensions;

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
        services.AddScoped<IVisitorRepository, VisitorFileRepository>();
    }
}