using Microsoft.Data.Sqlite;

namespace LinkShortener;
/// <summary>
/// The SQLite Db was causing consistent locking issues within prod so this is now deprecated in favour of cosmos
/// </summary>
internal class SQLiteRepository
{
    private readonly string _connexionString = "Data Source=linkShortener.sqlite" ;

    /// <summary>
    /// Write a link to the Db
    /// </summary>
    /// <param name="link">the link model</param>
    internal void WriteLink(ShortenedLinkModel link)
    {
        try
        {
            using var connexion = new SqliteConnection(_connexionString);
            connexion.Open();

            using (var command = new SqliteCommand(
                       "INSERT INTO ShortenedLinks (Id, OriginalLink) VALUES (@Id, @OriginalLink);", connexion))
            {
                command.Parameters.AddWithValue("@Hash", link.Id);
                command.Parameters.AddWithValue("@OriginalLink", link.OriginalLink);

                command.ExecuteNonQuery();
            }

            Console.WriteLine("Link inserted successfully.");
            connexion.Close();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error inserting link: {ex.Message}");
        }
    }
    
    /// <summary>
    /// Reads a link from the Db
    /// </summary>
    /// <param name="hash"></param>
    /// <returns></returns>
    internal ShortenedLinkModel ReadLink(string hash)
    {
        try
        {
            EnsureTableExists();
            using SqliteConnection connexion = new(_connexionString);
            connexion.Open();

            using var command = new SqliteCommand(
                "SELECT * FROM ShortenedLinks WHERE Id = @Id;", connexion);
            
            command.Parameters.AddWithValue("@Id", hash);

            using var reader = command.ExecuteReader();
            if (!reader.Read()) return null;
            
            return new ShortenedLinkModel
            { 
                Id = reader["Id"].ToString(), 
                OriginalLink = reader["OriginalLink"].ToString()
            };
        }
        catch (Exception ex)
        {
            throw new Exception($"Error querying link: {ex.Message}");
        }   
    }
    
    public void EnsureTableExists()
    {
        using (SqliteConnection connexion = new(_connexionString))
        {
            connexion.Open();

            using (SqliteCommand command = new(
                       @"CREATE TABLE IF NOT EXISTS ShortenedLinks (
                Id TEXT PRIMARY KEY,
                OriginalLink TEXT NOT NULL
            );", connexion))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
