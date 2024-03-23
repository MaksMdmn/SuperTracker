using System;
using System.Threading.Tasks;
using Bogus;
using MassTransit;
using Moq;
using StorageService.Api.DataContracts;

namespace StorageService.Api.Tests.Infrastructure.Consumers;

/// <summary>
/// I realize that the both projects must be fully covered with unit tests.
/// Hopefully you understand that I'm doing tech task in my spare time and still have the main job in parallel.
///
/// So I wanted to save time, but give you an understanding of what I'm doing and how and covered the consumer class.
/// </summary>
public class When_creating_visitor : VisitorConsumerFixtureBase
{
    [Fact]
    public async Task Should_create_visitor()
    {
        var command = GenerateCommand();
    
        await Consumer.Consume(command);
    
        AssertLogReceived(command);
        AssertMapped(command);
        AssertCreated();
    }
    
    [Fact]
    public async Task Should_create_visitor_with_ip_address_only()
    {
        var command = GenerateCommand();
        command.Message.Referrer = null;
        command.Message.UserAgent = null;
    
        await Consumer.Consume(command);
    
        AssertLogReceived(command);
        AssertMapped(command);
        AssertCreated();
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("    ")]
    public async Task Should_not_create_visitor_if_ip_address_null_or_empty(string ipAddress)
    {
        var command = GenerateCommand();
        command.Message.IpAddress = ipAddress;

        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            Consumer.Consume(command));
    
        AssertLogReceived(command);
        AssertNothingCreated();
    }

    void AssertLogReceived(ConsumeContext<CreateVisitor> command)
    {
        var message = $"Received message '{command.MessageId}' for visitor '{command.Message.IpAddress}'";
        
        AssertLogged(Times.Exactly(1), incoming => incoming == message);
    }
    
    static ConsumeContext<CreateVisitor> GenerateCommand()
    {
        var command = CreateVisitor.Generate();
        
        return Mock.Of<ConsumeContext<CreateVisitor>>(context => context.Message == command && 
                                                                 context.MessageId == Guid.NewGuid());
    }

    static readonly Faker<CreateVisitor> CreateVisitor = new Faker<CreateVisitor>()
        .RuleFor(command => command.IpAddress, rule => rule.Internet.IpAddress().ToString())
        .RuleFor(command => command.Referrer, rule => rule.Lorem.Word())
        .RuleFor(command => command.UserAgent, rule => rule.Lorem.Word());
}