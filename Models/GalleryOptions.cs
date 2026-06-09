namespace PhotoGallery.Models;

public sealed class GalleryOptions
{
    public const string SectionName = "Gallery";

    /// <summary>
    /// Base URL for index JSON and data files, e.g. https://account.blob.core.windows.net/photos/
    /// When empty, loads from the site's wwwroot via relative paths (data/...).
    /// </summary>
    public string DataBaseUrl { get; set; } = string.Empty;
}
