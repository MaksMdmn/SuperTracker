using System;

namespace StorageService.Api.Domain.Visitors;

/// <summary>
/// I expect this should be a business entity and have an Id 
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