using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using StorageService.Api.DataContracts;
using StorageService.Api.Domain.Visitors;

namespace StorageService.Api.Infrastructure.Messaging.Consumers;

public class CreateVisitorConsumer : IConsumer<CreateVisitor>
{
    readonly IVisitorRepository _visitors;

    readonly IMapper _mapper;
    readonly ILogger<CreateVisitorConsumer> _logger;

    public CreateVisitorConsumer(IVisitorRepository visitors, IMapper mapper, ILogger<CreateVisitorConsumer> logger)
    {
        _visitors = visitors;
        
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CreateVisitor> context)
    {
        _logger.LogInformation("Received message for visitor: {IpAddress}", context.Message.IpAddress);
        
        var visitor = _mapper.Map<Visitor>(context.Message);

        await _visitors.CreateAsync(visitor);
    }
}