namespace StorageService.Api.DataContracts;

public record CreateVisitor(string IpAddress, string? Referrer, string? UserAgent);