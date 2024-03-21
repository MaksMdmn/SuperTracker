using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PixelService.Api.Application.Interfaces;
using StorageService.Api.DataContracts;

namespace PixelService.Api.Application;

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
public class HttpDataCollector : IDataCollector
{
    readonly IHttpContextAccessor _accessor;

    readonly IBus _sender;

    readonly ILogger<HttpDataCollector> _logger;

    public HttpDataCollector(IHttpContextAccessor accessor, IBus sender, ILogger<HttpDataCollector> logger)
    {
        _accessor = accessor;
        _sender = sender;
        _logger = logger;
    }

    public async Task CollectAsync()
    {
        var ip = _accessor.HttpContext!.Connection.RemoteIpAddress?.ToString();
        var referrer = _accessor.HttpContext.Request.Headers.Referer;
        var userAgent = _accessor.HttpContext.Request.Headers.UserAgent;
        
        if (string.IsNullOrWhiteSpace(ip))
        {
            _logger.LogError(
                "Ip address cannot be identified, request '{TraceId}' skipped", 
                _accessor.HttpContext.TraceIdentifier);
            
            return;
        }

        var command = new CreateVisitor(ip, referrer, userAgent);
        await _sender.Publish(command);
    }
}