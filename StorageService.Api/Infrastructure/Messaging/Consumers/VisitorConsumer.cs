using System;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using StorageService.Api.DataContracts;
using StorageService.Api.Domain.Visitors;

namespace StorageService.Api.Infrastructure.Messaging.Consumers;

public class VisitorConsumer : IConsumer<CreateVisitor>
{
    readonly IVisitorRepository _visitors;

    readonly IMapper _mapper;
    readonly ILogger<VisitorConsumer> _logger;

    public VisitorConsumer(IVisitorRepository visitors, IMapper mapper, ILogger<VisitorConsumer> logger)
    {
        _visitors = visitors;
        
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<CreateVisitor> context)
    {
        LogReceived(context);

        Validate(context.Message);
        
        var visitor = _mapper.Map<Visitor>(context.Message);

        await _visitors.CreateAsync(visitor);
    }

    void LogReceived(ConsumeContext<CreateVisitor> context)
    {
        _logger.LogInformation(
            "Received message '{MessageId}' for visitor '{IpAddress}'",
            context.MessageId,
            context.Message.IpAddress ?? string.Empty);
    }

    ///
    /// Potentially a FluentValidation could be used instead. I've just kept it simple
    /// 
    static void Validate(CreateVisitor message)
    {
        if (string.IsNullOrWhiteSpace(message.IpAddress))
            throw new InvalidOperationException("Ip address cannot be null");
    }
}