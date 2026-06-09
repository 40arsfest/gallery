using System.Globalization;
using PhotoGallery.Models;

namespace PhotoGallery.Services;

public static class PhotoQueries
{
    public static IEnumerable<AlbumSummary> OrderAlbums(IEnumerable<AlbumSummary> albums) =>
        albums
            .OrderByDescending(a => TryParseIsoDate(a.Date) ?? DateOnly.MinValue)
            .ThenBy(a => a.AlbumTitle, StringComparer.OrdinalIgnoreCase);

    public static string? FormatDisplayDate(string? isoOrNull)
    {
        if (string.IsNullOrEmpty(isoOrNull))
            return null;

        return DateOnly.TryParse(isoOrNull, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
            ? date.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)
            : isoOrNull;
    }

    public static DateOnly? TryParseIsoDate(string? isoOrNull) =>
        string.IsNullOrEmpty(isoOrNull)
            ? null
            : DateOnly.TryParse(isoOrNull, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
                ? date
                : null;

    public static string? FormatFileSizeMb(long? bytes) =>
        bytes is > 0
            ? $"{bytes.Value / 1024.0 / 1024.0:0.##} MB"
            : null;

    public static string? FormatResolution(int? width, int? height) =>
        width is > 0 && height is > 0
            ? $"{width} × {height} px"
            : null;

    /// <summary>
    /// "fest hos pelle 24-05-2004" → "Fest hos pelle"
    /// </summary>
    public static string FormatAlbumTitle(string album)
    {
        var date = PhotoPaths.TryExtractDateFromAlbum(album);
        if (date is not null && album.EndsWith(date, StringComparison.Ordinal))
        {
            var title = album[..^(date.Length)].TrimEnd();
            if (title.Length > 0)
                return char.ToUpper(title[0]) + title[1..];
        }

        return album.Length > 0
            ? char.ToUpper(album[0]) + album[1..]
            : album;
    }
}
