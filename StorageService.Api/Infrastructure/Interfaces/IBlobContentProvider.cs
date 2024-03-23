namespace StorageService.Api.Infrastructure.Interfaces;

public interface IBlobContentProvider
{
    byte[] Download(string url);
}