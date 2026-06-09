using System.Globalization;

namespace PhotoGallery.Models;

/// <summary>
/// Mappstruktur: {år}/{album}/{filnamn}
/// Exempel: 2004/fest hos pelle 24-05-2004/img1.jpg
/// </summary>
public static class PhotoPaths
{
    public const char Separator = '/';
    private const string AlbumDateFormat = "dd-MM-yyyy";

    public static string GetRelativePath(PhotoItem photo) =>
        string.Join(Separator, photo.Year, photo.Album, photo.FileName);

    public static string GetRelativePath(string year, string album, string fileName) =>
        string.Join(Separator, year, album, fileName);

    public static string GetBlobUrl(string containerBaseUrl, PhotoItem photo) =>
        $"{containerBaseUrl.TrimEnd('/')}/{GetRelativePath(photo)}";

    public static string GetBlobUrl(string containerBaseUrl, string relativePath) =>
        $"{containerBaseUrl.TrimEnd('/')}/{relativePath}";

    /// <summary>
    /// Parsar en relativ sökväg till foto-metadata.
    /// </summary>
    public static PhotoItem FromRelativePath(string relativePath) =>
        FromRelativePath(relativePath, PlaceholderUrls);

    public static PhotoItem FromRelativePath(
        string relativePath,
        Func<string, (string ThumbnailUrl, string ImageUrl)> urlFactory)
    {
        var parts = relativePath.Split(Separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length != 3)
            throw new ArgumentException(
                $"Ogiltig sökväg. Förväntat format: år/album/filnamn. Fick: {relativePath}",
                nameof(relativePath));

        var year = parts[0];
        var album = parts[1];
        var fileName = parts[2];
        var id = $"{year}-{Slugify(album)}-{Path.GetFileNameWithoutExtension(fileName)}";
        var urls = urlFactory(id);

        return new PhotoItem(
            Id: id,
            Year: year,
            Album: album,
            FileName: fileName,
            ThumbnailUrl: urls.ThumbnailUrl,
            ImageUrl: urls.ImageUrl,
            Date: TryExtractDateFromAlbum(album));
    }

    public static (string ThumbnailUrl, string ImageUrl) PlaceholderUrls(string id) => (
        $"https://picsum.photos/seed/{id}/400/300",
        $"https://picsum.photos/seed/{id}/1600/1000");

    /// <summary>
    /// Datum i albumnamnet, t.ex. "24-05-2004" från "fest hos pelle 24-05-2004".
    /// </summary>
    public static string? TryExtractDateFromAlbum(string album)
    {
        var lastSpace = album.LastIndexOf(' ');
        if (lastSpace < 0)
            return null;

        var candidate = album[(lastSpace + 1)..];
        return DateOnly.TryParseExact(candidate, AlbumDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _)
            ? candidate
            : null;
    }

    public static DateOnly? TryParseAlbumDate(string album)
    {
        var dateText = TryExtractDateFromAlbum(album);
        if (dateText is null)
            return null;

        return DateOnly.TryParseExact(dateText, AlbumDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
            ? date
            : null;
    }

    private static string Slugify(string value) =>
        string.Concat(value.ToLowerInvariant().Where(char.IsLetterOrDigit));
}
