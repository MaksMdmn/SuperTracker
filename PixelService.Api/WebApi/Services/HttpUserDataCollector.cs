using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
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
///
/// - requirements do not specify any for choice of the communication approach, so I'm using masstransit here.
/// It supports most of the popular buses (or event-streaming(s) like kafka, azure event hub, in case if we're gonna
/// have lots of visitors), so we can later easily switch between them 
/// </summary>
public class HttpUserDataCollector(
    IHttpContextAccessor accessor, 
    IMessageSender sender, 
    ILogger<HttpUserDataCollector> logger)
    : IUserDataCollector
{
    public async Task CollectAsync()
    {
        var ip = accessor.HttpContext!.Connection.RemoteIpAddress?.ToString();
        var referrer = accessor.HttpContext.Request.Headers.Referer;
        var userAgent = accessor.HttpContext.Request.Headers.UserAgent;
        
        if (string.IsNullOrWhiteSpace(ip))
        {
            logger.LogError(
                "Ip address cannot be identified, request '{TraceId}' skipped", 
                accessor.HttpContext.TraceIdentifier);
            
            return;
        }

        var command = new CreateVisitor(ip, referrer, userAgent);
        await sender.SendAsync(command);
    }
}