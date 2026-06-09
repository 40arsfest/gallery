namespace PhotoGallery.Models;

public sealed record AlbumPhotosResult(
    string AlbumTitle,
    string Album,
    IReadOnlyList<PhotoItem> Photos);
