namespace PhotoGallery.Models;

public sealed record AlbumSummary(
    string Year,
    string Album,
    string AlbumSlug,
    string AlbumTitle,
    string? Date,
    int PhotoCount,
    string? CoverThumbnailUrl);
