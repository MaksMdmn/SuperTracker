using System.Threading.Tasks;

namespace StorageService.Api.Infrastructure.Interfaces;

public interface IMessageSender
{
    Task SendAsync<TMessage>(TMessage message) where TMessage : class;
}