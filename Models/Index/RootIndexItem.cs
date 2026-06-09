using System.Text.Json.Serialization;

namespace PhotoGallery.Models.Index;

public sealed class RootIndexItem
{
    [JsonPropertyName("year")]
    public required string Year { get; init; }

    [JsonPropertyName("albumCount")]
    public int AlbumCount { get; init; }

    [JsonPropertyName("photoCount")]
    public int PhotoCount { get; init; }

    [JsonPropertyName("indexUrl")]
    public required string IndexUrl { get; init; }
}
