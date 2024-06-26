using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PixelService.Api.Application.Interfaces;
using PixelService.Api.Infrastructure.Interfaces;
using StorageService.Api.DataContracts;

namespace PixelService.Api.WebApi.Services;

/// <summary>
/// I decided to go with messaging communication between the components:
/// 
/// - pixel-service can be up scaled or down scaled, but storage-service can't. With messaging approach, we can control
/// storage service load independently (e.g. by the size of batch of messages we are able to process per second or so)
/// 
/// - if storage-service would appear to be a bottleneck and fail, pixel-service will continue their job with no
/// interruptions
/// </summary>
public class HttpUserDataCollector(
    IHttpContextAccessor accessor, 
    IMessageSender sender)
    : IUserDataCollector
{
    public async Task CollectAsync()
    {
        var ip = accessor.HttpContext!.Connection.RemoteIpAddress?.ToString();
        var referrer = accessor.HttpContext.Request.Headers.Referer;
        var userAgent = accessor.HttpContext.Request.Headers.UserAgent;
        
        var command = new CreateVisitor
        {
            IpAddress = ip,
            Referrer = referrer,
            UserAgent = userAgent
        };
        
        await sender.SendAsync(command);
    }
}