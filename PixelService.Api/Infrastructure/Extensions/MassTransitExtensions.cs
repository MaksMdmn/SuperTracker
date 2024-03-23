using System;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PixelService.Api.Infrastructure.Interfaces;
using PixelService.Api.Infrastructure.Messaging;

namespace PixelService.Api.Infrastructure.Extensions;

public static class MassTransitExtensions
{
    public static void AddMassTransitServices(this IServiceCollection services, string url)
    {
        services.AddScoped<IMessageSender, MassTransitMessageSender>();

        services.AddMassTransit<IBus>(options =>
        {
            options.SetKebabCaseEndpointNameFormatter();
    
            options.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(url));

                configurator.ConfigureEndpoints(context);
            });
        });
    }
}