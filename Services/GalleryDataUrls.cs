using Microsoft.Extensions.Options;
using PhotoGallery.Models;

namespace PhotoGallery.Services;

public sealed class GalleryDataUrls(IOptions<GalleryOptions> options)
{
    private readonly GalleryOptions _options = options.Value;

    public string Resolve(string relativeOrAbsoluteUrl)
    {
        if (relativeOrAbsoluteUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
            || relativeOrAbsoluteUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return relativeOrAbsoluteUrl;
        }

        var path = relativeOrAbsoluteUrl.TrimStart('/');
        var baseUrl = _options.DataBaseUrl?.Trim();

        if (string.IsNullOrEmpty(baseUrl))
            return path;

        return $"{baseUrl.TrimEnd('/')}/{path}";
    }
}
