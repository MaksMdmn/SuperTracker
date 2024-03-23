using System.Threading.Tasks;
using MassTransit;
using PixelService.Api.Infrastructure.Interfaces;

namespace PixelService.Api.Infrastructure.Messaging;

public class MassTransitMessageSender(IBus sender) : IMessageSender
{
    public async Task SendAsync<TMessage>(TMessage message) where TMessage : class
    {
        await sender.Publish(message);
    }
}