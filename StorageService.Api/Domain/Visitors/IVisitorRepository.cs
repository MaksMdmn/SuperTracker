using System.Threading.Tasks;

namespace StorageService.Api.Domain.Visitors;

public interface IVisitorRepository
{
    Task CreateAsync(Visitor visitor);
}