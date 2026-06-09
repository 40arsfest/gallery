using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PhotoGallery.Models;
using PhotoGallery.Models.Index;

namespace PhotoGallery.Services;

public sealed class BlobGalleryPhotoService(
    HttpClient httpClient,
    GalleryDataUrls dataUrls) : IPhotoService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private List<YearSummary>? _yearsCache;
    private readonly Dictionary<string, List<AlbumSummary>> _albumsCache = new(StringComparer.Ordinal);
    private readonly Dictionary<string, AlbumPhotosResult> _albumPhotosCache = new(StringComparer.Ordinal);

    public async Task<IReadOnlyList<YearSummary>> GetYearsAsync(CancellationToken cancellationToken = default)
    {
        if (_yearsCache is not null)
            return _yearsCache;

        var items = await httpClient.GetFromJsonAsync<List<RootIndexItem>>(
            dataUrls.Resolve("data/index.json"),
            JsonOptions,
            cancellationToken) ?? [];

        _yearsCache = items
            .Select(i => new YearSummary(i.Year, i.AlbumCount, i.PhotoCount))
            .OrderBy(y => y.Year, StringComparer.Ordinal)
            .ToList();

        return _yearsCache;
    }

    public async Task<IReadOnlyList<AlbumSummary>> GetAlbumsAsync(
        string year,
        CancellationToken cancellationToken = default)
    {
        if (_albumsCache.TryGetValue(year, out var cached))
            return cached;

        var items = await httpClient.GetFromJsonAsync<List<YearIndexItem>>(
            dataUrls.Resolve($"data/{year}/index.json"),
            JsonOptions,
            cancellationToken) ?? [];

        var albums = items
            .Select(i => new AlbumSummary(
                i.Year,
                i.Album,
                i.AlbumSlug,
                i.AlbumTitle,
                i.Date,
                i.PhotoCount,
                i.CoverThumbnailUrl))
            .ToList();

        _albumsCache[year] = albums;
        return albums;
    }

    public async Task<AlbumPhotosResult?> GetAlbumPhotosAsync(
        string year,
        string albumSlug,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = $"{year}/{albumSlug}";
        if (_albumPhotosCache.TryGetValue(cacheKey, out var cached))
            return cached;

        var items = await httpClient.GetFromJsonAsync<List<AlbumPhotoItem>>(
            dataUrls.Resolve($"data/{year}/{albumSlug}.json"),
            JsonOptions,
            cancellationToken);

        if (items is null || items.Count == 0)
            return null;

        var first = items[0];
        var photos = items
            .Select(p => new PhotoItem(
                p.Id,
                p.Year,
                p.Album,
                p.FileName,
                p.ThumbnailUrl,
                p.ImageUrl,
                p.Date,
                p.FileSizeBytes > 0 ? p.FileSizeBytes : null,
                p.Width,
                p.Height,
                p.CameraModel))
            .ToList();

        var result = new AlbumPhotosResult(first.AlbumTitle, first.Album, photos);
        _albumPhotosCache[cacheKey] = result;
        return result;
    }

    public async Task<PhotoItem?> GetRandomPhotoAsync(CancellationToken cancellationToken = default)
    {
        var years = await GetYearsAsync(cancellationToken);
        if (years.Count == 0)
            return null;

        var year = PickWeighted(years, y => y.PhotoCount);
        var albums = await GetAlbumsAsync(year.Year, cancellationToken);
        if (albums.Count == 0)
            return null;

        var album = PickWeighted(albums, a => a.PhotoCount);
        var albumPhotos = await GetAlbumPhotosAsync(year.Year, album.AlbumSlug, cancellationToken);
        if (albumPhotos is null || albumPhotos.Photos.Count == 0)
            return null;

        return albumPhotos.Photos[Random.Shared.Next(albumPhotos.Photos.Count)];
    }

    private static T PickWeighted<T>(IReadOnlyList<T> items, Func<T, int> weightSelector)
    {
        var total = items.Sum(weightSelector);
        if (total <= 0)
            return items[Random.Shared.Next(items.Count)];

        var pick = Random.Shared.Next(total);
        var sum = 0;
        foreach (var item in items)
        {
            sum += weightSelector(item);
            if (pick < sum)
                return item;
        }

        return items[^1];
    }
}
