using System;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using StorageService.Api.DataContracts;
using StorageService.Api.Domain.Visitors;
using StorageService.Api.Infrastructure.Messaging.Consumers;

namespace StorageService.Api.Tests.Infrastructure.Consumers;

public class VisitorConsumerFixtureBase
{
    protected readonly Mock<IVisitorRepository> Repository;
    protected readonly Mock<IMapper> Mapper;
    protected readonly Mock<ILogger<VisitorConsumer>> Logger;
    
    protected readonly IConsumer<CreateVisitor> Consumer;
    
    public VisitorConsumerFixtureBase()
    {
        Repository = new Mock<IVisitorRepository>();
        Mapper = new Mock<IMapper>();
        Logger = new Mock<ILogger<VisitorConsumer>>();

        Consumer = new VisitorConsumer(Repository.Object, Mapper.Object, Logger.Object);
        
        SetupLogger();
        SetupRepository();
    }

    protected void SetupLogger()
    {
        Logger
            .Setup(logger => logger.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(), 
                It.IsAny<object>(),
                It.IsAny<Exception>(), 
                It.IsAny<Func<object, Exception, string>>()))
            .Verifiable();
    }

    protected void SetupRepository()
    {
        Repository
            .Setup(r => r.CreateAsync(It.IsAny<Visitor>()))
            .Returns(Task.CompletedTask);
    }
    
    protected void AssertCreated()
    {
        Repository.Verify(
            x => x.CreateAsync(It.IsAny<Visitor>()),
            Times.Once);
    }
    
    protected void AssertNothingCreated()
    {
        Repository.Verify(
            x => x.CreateAsync(It.IsAny<Visitor>()),
            Times.Never);
    }

    protected void AssertMapped(ConsumeContext<CreateVisitor> command)
    {
        Mapper.Verify(
            x => x.Map<Visitor>(command.Message),
            Times.Once);
    }

    protected void AssertLogged(Times times, Predicate<string> message)
    {
        Logger.Verify(x => x.Log(
                It.IsAny<LogLevel>(), 
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((input, type) => Match(input, message)),
                It.IsAny<Exception>(), (Func<It.IsAnyType, Exception, string>) It.IsAny<object>()),
            times);
    }
    
    static bool Match(object o, Predicate<string> match)
    {
        return match(o?.ToString());
    }
}