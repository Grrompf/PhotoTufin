using System.Data.SQLite;
using Dapper;

namespace PhotoTufin.Repository;

public abstract class BaseRepository : IRepository
{
    /// <summary>
    /// SQLite database file is in the application directory. The file name is the trimmed Productname of the application as
    /// defined in App.xaml.cs, e.g. "PhotoTufin.sqlite" 
    /// </summary>
    private static string DbFile => $"{App.DataBaseFile}";

    /// <summary>
    /// Creates a connection to the database. If datebase is not existing, it will be created.
    /// </summary>
    /// <returns></returns>
    protected static SQLiteConnection DbConnection()
    {
        return new SQLiteConnection("Data Source=" + DbFile);
    }

    /// <summary>
    /// Open a database connection and execute an sql statement. Finally, the connection is closed.
    /// </summary>
    /// <param name="sql"></param>
    protected static void ExecDbCmd(string sql)
    {
        using var conn = DbConnection();
        conn.Open();
        conn.Execute(sql);
        conn.Close();
    }
    
    public abstract void CreateTable();

    public abstract void DropTable();
}