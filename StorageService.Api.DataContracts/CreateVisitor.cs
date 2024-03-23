namespace StorageService.Api.DataContracts;

/// <summary>
/// Even considering I've already simplified a usual onion architecture by separating layer by folders - I've decided
/// to data contracts in a separate solution to avoid any references between PixelService and StorageService projects
/// </summary>
public record CreateVisitor(string IpAddress, string Referrer, string UserAgent);