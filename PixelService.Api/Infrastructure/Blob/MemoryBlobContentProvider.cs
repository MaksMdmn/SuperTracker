using System;
using PixelService.Api.Infrastructure.Interfaces;

namespace PixelService.Api.Infrastructure.Blob;

/// <summary>
/// This was not in requirements of where to get/store image.
/// In this case I just hard code it in memory, potentially we could store it as static file and/or, if there re
/// much more image options - in Azure blob storage (and provide implementation for that provider accordingly).
/// </summary>
public class MemoryBlobContentProvider : IBlobContentProvider
{
    public byte[] Download(string url)
    {
        return Convert.FromBase64String(Transparent1X1PixelImageGif);
    }
    
    const string Transparent1X1PixelImageGif = "R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";
}