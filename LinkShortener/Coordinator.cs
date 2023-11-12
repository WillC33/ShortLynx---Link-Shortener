namespace LinkShortener;

/// <summary>
/// Coordinates link shortening and reading options
/// </summary>
internal class Coordinator(ShortenerService shortener, Repository repository)
{
    /// <summary>
    /// Shortens and writes a link to the Db
    /// </summary>
    /// <param name="link">the link</param>
    /// <returns>the hashed linked</returns>
    internal string WriteLink(string link)
    {
        var hash = shortener.ShortenLink(link);
        
        ShortenedLinkModel model = new()
        {
            hash = hash,
            originalLink = link
        };
        
        repository.WriteLink(model);
        return hash;
    }

    /// <summary>
    /// Reads a hash from the Db and returns the link
    /// </summary>
    /// <param name="hash">the hash</param>
    /// <returns>the original link</returns>
    internal string ReadLink(string hash)
    {
        var model = repository.ReadLink(hash);
        return model?.originalLink;
    }
}