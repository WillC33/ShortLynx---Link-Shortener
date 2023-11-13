namespace LinkShortener;

/// <summary>
/// Coordinates link shortening and reading options
/// </summary>
internal class Coordinator(ShortenerService shortener, CosmosRepository repository)
{
    private const string BaseUrl = "https://shortlynx.azurewebsites.net/go/";
    
    /// <summary>
    /// Shortens and writes a link to the Db
    /// </summary>
    /// <param name="link">the link</param>
    /// <returns>the hashed linked</returns>
    internal async Task<string> WriteLink(string link)
    {
        var hash = shortener.ShortenLink(link);
        
        ShortenedLinkModel model = new()
        {
            Id = hash,
            OriginalLink = link
        };
        
        await repository.WriteLink(model);
        return $"{BaseUrl}{hash}";
    }

    /// <summary>
    /// Reads a hash from the Db and returns the link
    /// </summary>
    /// <param name="hash">the hash</param>
    /// <returns>the original link</returns>
    internal async Task<string> ReadLink(string hash)
    {
        var model = await repository.ReadLink(hash);
        return model?.OriginalLink;
    }
}