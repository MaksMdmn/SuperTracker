using System;

namespace StorageService.Api.Domain.Visitors;

/// <summary>
/// This is my guess, but Visitor might probably has much more business logic around it.
/// So I extracted it as a business entity within the Domain layer and added an Id to it.
///
/// The business view of this entity does not impact the format of file you requested in requirements though
/// datetime|referrer|useragent|ip
/// </summary>
public class Visitor
{
    public Guid Id { get; }

    public DateTime VisitedAt { get;  }
    
    public string IpAddress { get; }
    public string Referrer { get; }
    public string UserAgent { get; }

    
    public Visitor(string ipAddress, string referrer = null, string userAgent = null) 
    {
        Id = Guid.NewGuid();
        VisitedAt = DateTime.UtcNow;
        
        IpAddress = ipAddress;
        Referrer = referrer;
        UserAgent = userAgent;
    }

    public Visitor(Guid id, DateTime visitedAt, string ipAddress, string referrer, string userAgent)
    {
        Id = id;
        VisitedAt = visitedAt;
        IpAddress = ipAddress;
        Referrer = referrer;
        UserAgent = userAgent;
    }
}