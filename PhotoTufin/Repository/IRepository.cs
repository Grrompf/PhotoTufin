namespace PhotoTufin.Repository;

public interface IRepository
{
    /// <summary>
    /// Deletes all data.   
    /// </summary>
    void DeleteAllData();
    
    /// <summary>
    /// Creates a table if not existing.
    /// </summary>
    void CreateTable();
    
    /// <summary>
    /// Delete table if existing.
    /// </summary>
    void DropTable();
}