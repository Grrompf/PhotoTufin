namespace PhotoTufin.Repository;

public interface IRepository
{
    /// <summary>
    /// Creates a table if not existing.
    /// </summary>
    void CreateTable();
    
    /// <summary>
    /// Delete table if existing.
    /// </summary>
    void DropTable();
}