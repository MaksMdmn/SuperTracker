using System;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using StorageService.Api.Infrastructure.Mappers;
using StorageService.Api.Infrastructure.Messaging.Consumers;

namespace StorageService.Api.Infrastructure.Extensions;

public static class MassTransitExtensions
{
    public static void AddMassTransitServices(this IServiceCollection services, string url)
    {
        services.AddAutoMapper(typeof(MessagingProfile));

        services.AddMassTransit<IBus>(options =>
        {
            options.SetKebabCaseEndpointNameFormatter();
            
            options.AddConsumer<CreateVisitorConsumer>();
    
            options.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(url));

                configurator.ConfigureEndpoints(context);
            });
        });
    }
}