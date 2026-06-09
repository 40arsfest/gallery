using System.Text.Json.Serialization;

namespace PhotoGallery.Models.Index;

public sealed class YearIndexItem
{
    [JsonPropertyName("year")]
    public required string Year { get; init; }

    [JsonPropertyName("album")]
    public required string Album { get; init; }

    [JsonPropertyName("albumSlug")]
    public required string AlbumSlug { get; init; }

    [JsonPropertyName("albumTitle")]
    public required string AlbumTitle { get; init; }

    [JsonPropertyName("date")]
    public string? Date { get; init; }

    [JsonPropertyName("photoCount")]
    public int PhotoCount { get; init; }

    [JsonPropertyName("coverThumbnailUrl")]
    public string? CoverThumbnailUrl { get; init; }

    [JsonPropertyName("photosUrl")]
    public required string PhotosUrl { get; init; }
}
