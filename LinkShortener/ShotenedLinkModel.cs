namespace LinkShortener;

/// <summary>
/// The link model for the Db
/// </summary>
internal record ShortenedLinkModel()
{
    internal string hash { get; set; }
    internal string originalLink { get; set; }
    
}