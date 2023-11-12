using System.Security.Cryptography;
using System.Text;

namespace LinkShortener;

/// <summary>
/// Deals with shortening the links
/// </summary>
internal class ShortenerService
{
    /// <summary>
    /// Returns a hashed version of the link
    /// </summary>
    /// <param name="link">the link</param>
    /// <returns>the hash</returns>
    internal string ShortenLink(string link)
    {
        //TODO: this doesn't really shorten the links so the logic works but this method is not right
        byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(link));

        //convert to hexadecimal string
        StringBuilder stringBuilder = new();
        foreach (byte b in hashBytes)
        {
            stringBuilder.Append(b.ToString("x2"));
        }

        return stringBuilder.ToString();
    }
}