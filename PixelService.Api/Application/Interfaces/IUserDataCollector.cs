using System.Threading.Tasks;

namespace PixelService.Api.Application.Interfaces;

public interface IUserDataCollector
{
    Task CollectAsync();
}