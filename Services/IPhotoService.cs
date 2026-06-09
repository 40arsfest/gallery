using PhotoGallery.Models;

namespace PhotoGallery.Services;

public interface IPhotoService
{
    Task<IReadOnlyList<YearSummary>> GetYearsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AlbumSummary>> GetAlbumsAsync(string year, CancellationToken cancellationToken = default);

    Task<AlbumPhotosResult?> GetAlbumPhotosAsync(
        string year,
        string albumSlug,
        CancellationToken cancellationToken = default);

    Task<PhotoItem?> GetRandomPhotoAsync(CancellationToken cancellationToken = default);
}
