using System.Data.SQLite;
using Dapper;

namespace PhotoTufin.Data;

public abstract class BaseRepository : IRepository
{
    /// <summary>
    /// SQLite database file is in the application directory. The file name is the trimmed Productname of the application as
    /// defined in App.xaml.cs, e.g. "PhotoTufin.sqlite" 
    /// </summary>
    protected static string DbFile => $"{App.DataBaseFile}";

    /// <summary>
    /// Creates a connection to the database. If datebase is not existing, it will be created.
    /// </summary>
    /// <returns></returns>
    protected static SQLiteConnection DbConnection()
    {
        return new SQLiteConnection("Data Source=" + DbFile);
    }

    /// <summary>
    /// Creates a table if not existing.
    /// </summary>
    public void CreateTable()
    {
        var sql = CreateTableSQL();
        ExecDbCmd(sql);
    }
    
    /// <summary>
    /// Drops (Delete) a table if existing.
    /// </summary>
    public void DropTable()
    {
        var sql = DropTableSQL();
        ExecDbCmd(sql);
    }

    /// <summary>
    /// Generic sql execution by dapper.
    /// </summary>
    /// <param name="sql"></param>
    private static void ExecDbCmd(string sql)
    {
        using var conn = DbConnection();
        conn.Open();
        conn.Execute(sql);
    }
    
    public abstract string DropTableSQL();
    public abstract string CreateTableSQL();
}