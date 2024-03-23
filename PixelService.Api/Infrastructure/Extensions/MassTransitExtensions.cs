using System;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using PixelService.Api.Infrastructure.Interfaces;
using PixelService.Api.Infrastructure.Messaging;

namespace PixelService.Api.Infrastructure.Extensions;
/// <summary>
/// As in requirements that communication protocol must be a choice of mine - I've used masstransit due to:
/// it provides additional abstraction layer on top of messaging, so we can easily switch from rabbitmq (like in
/// my case) to another bus of even event streaming (for now it supports Azure Event Hubs and Kafka).
///
/// This flexibility is important to achieve as requirements do not mention anything about
/// - the load
/// - the performance 
/// - the importance of data (is risk of losing data is acceptable in favour of performance?) 
/// </summary>
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