using System.Text.Json.Serialization;

namespace PhotoGallery.Models.Index;

public sealed class AlbumPhotoItem
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }

    [JsonPropertyName("year")]
    public required string Year { get; init; }

    [JsonPropertyName("album")]
    public required string Album { get; init; }

    [JsonPropertyName("albumTitle")]
    public required string AlbumTitle { get; init; }

    [JsonPropertyName("date")]
    public string? Date { get; init; }

    [JsonPropertyName("fileName")]
    public required string FileName { get; init; }

    [JsonPropertyName("thumbnailUrl")]
    public required string ThumbnailUrl { get; init; }

    [JsonPropertyName("imageUrl")]
    public required string ImageUrl { get; init; }

    [JsonPropertyName("fileSizeBytes")]
    public long FileSizeBytes { get; init; }

    [JsonPropertyName("width")]
    public int? Width { get; init; }

    [JsonPropertyName("height")]
    public int? Height { get; init; }

    [JsonPropertyName("cameraModel")]
    public string? CameraModel { get; init; }
}
