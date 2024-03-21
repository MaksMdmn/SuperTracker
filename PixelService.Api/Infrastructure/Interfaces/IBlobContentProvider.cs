namespace PixelService.Api.Infrastructure.Interfaces;

public interface IBlobContentProvider
{
    byte[] Download(string url);
}