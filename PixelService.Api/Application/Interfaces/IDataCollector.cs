using System.Threading.Tasks;

namespace PixelService.Api.Application.Interfaces;

public interface IDataCollector
{
    Task CollectAsync();
}