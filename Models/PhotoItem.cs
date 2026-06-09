namespace PhotoGallery.Models;

public sealed record PhotoItem(
    string Id,
    string Year,
    string Album,
    string FileName,
    string ThumbnailUrl,
    string ImageUrl,
    string? Date = null,
    long? FileSizeBytes = null,
    int? Width = null,
    int? Height = null,
    string? CameraModel = null);
