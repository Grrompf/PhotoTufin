namespace PhotoTufin.Data;

public interface IRepository
{
    /// <summary>
    /// You must implement the sql command for creating a table
    /// e.g. @"CREATE TABLE IF NOT EXISTS _tblName (ID INTEGER PRIMARY KEY , ...)";
    /// </summary>
    /// <returns>string</returns>
    string CreateTableSQL();
    
    /// <summary>
    /// You must implement the sql command for dropping a table e.g. @"DROP TABLE IF EXISTS _tblName;"
    /// </summary>
    /// <returns>string</returns>
    string DropTableSQL();
}