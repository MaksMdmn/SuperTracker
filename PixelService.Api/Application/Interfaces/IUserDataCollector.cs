using System.Threading.Tasks;

namespace PixelService.Api.Application.Interfaces;
/// <summary>
/// Also, requirements does not specify of what project structure to choose, but just say to keep it simple
/// So I went with an simplified 'ONION' approach.
/// But, considering the simplicity of the solution, I kept everything in one project and separated onion layers by
/// folders
///
/// I haven't seen any need to provide a domain entities for Pixel Service so Domain folder is kept empty, but was
/// created for consistency. 
/// </summary>
public interface IUserDataCollector
{
    Task CollectAsync();
}