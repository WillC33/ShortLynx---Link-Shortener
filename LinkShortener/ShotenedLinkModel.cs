using Newtonsoft.Json;

namespace LinkShortener;

/// <summary>
/// The link model for the Db
/// </summary>
internal record ShortenedLinkModel()
{
    [JsonProperty("id")]
    internal string Id { get; set; }
    internal string OriginalLink { get; set; }
    
}