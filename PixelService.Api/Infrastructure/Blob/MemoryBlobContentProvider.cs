using System;
using PixelService.Api.Infrastructure.Interfaces;

namespace PixelService.Api.Infrastructure.Blob;

/// <summary>
/// This was not in requirements of where to get/store image.
/// In this case I just hardcoded it in memory for simplification.
/// But in case if we might have more complex logic and/or different images to collect data - potentially it can be
/// reimplemented to pick up from:
/// - static files
/// - cloud blob storage
/// - etc.
/// </summary>
public class MemoryBlobContentProvider : IBlobContentProvider
{
    public byte[] Download(string url)
    {
        return Convert.FromBase64String(Transparent1X1PixelImageGif);
    }
    
    const string Transparent1X1PixelImageGif = "R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";
}