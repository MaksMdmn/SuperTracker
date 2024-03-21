using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using StorageService.Api.Domain.Visitors;
using StorageService.Api.MessageApi.Settings;

namespace StorageService.Api.Infrastructure;

public class VisitorFileRepository : IVisitorRepository
{   
    readonly string _file;
    
    public VisitorFileRepository(IOptions<AppSettings> settings)
    {
        _file = Path.Combine(settings.Value.FilePath ?? DefaultFilePath, FileName);
    }
    
    public async Task CreateAsync(Visitor visitor)
    {
        var model = Map(visitor);

        await File.AppendAllLinesAsync(_file, model);
    }

    static string[] Map(Visitor visitor)
    {
        const char delimiter = '|';
        const string dateTimeFormat = "O";

        var result = new StringBuilder()
            .Append(visitor.VisitedAt.ToString(dateTimeFormat))
            .Append(delimiter)
            .Append(visitor.Referrer)
            .Append(delimiter)
            .Append(visitor.UserAgent)
            .Append(delimiter)
            .Append(visitor.IpAddress);

        return [result.ToString()];
    }
    
    const string DefaultFilePath = "/tmp";
    const string FileName = "visits.log";
}