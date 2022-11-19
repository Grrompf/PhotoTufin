using System.Collections.Generic;
using System.Linq;
using Dapper;
using PhotoTufin.Model;

namespace PhotoTufin.Repository;

public class DiskInfoRepository : BaseRepository, IDiskInfoRepository
{
    /// <summary>
    /// By Using the constructor, database and table will be created if not existing.  
    /// </summary>
    public DiskInfoRepository()
    {
        CreateTable();
    }
    
    /// <summary>
    /// Returns the model by its primary key or null if not found.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>DiskInfo?</returns>
    public DiskInfo? Find(long id)
    {
        using var conn = DbConnection();
        conn.Open();
        
        var result = conn.QueryFirstOrDefault<DiskInfo>(
            @"SELECT * FROM DiskInfo WHERE Id = @Id",
            new { id }
        );
        conn.Close();

        return result;
    }
    
    /// <summary>
    /// Returns the model by its unique key or null if not found.
    /// </summary>
    /// <param name="serialNo"></param>
    /// <returns></returns>
    public DiskInfo? FindBySerialNo(string serialNo)
    {
        using var conn = DbConnection();
        conn.Open();
        
        var result = conn.QueryFirstOrDefault<DiskInfo>(
            @"SELECT * FROM DiskInfo WHERE SerialNo = @serialNo",
            new { serialNo }
            );
        conn.Close();

        return result;
    }
    
    /// <summary>
    /// Returns the model by its unique key or null if not found.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public static int GetModelCount(string model)
    {
        using var conn = DbConnection();
        conn.Open();
        
        var result = conn.ExecuteScalar<int>(
            @"SELECT COUNT(*) FROM DiskInfo WHERE Model = @model",
            new { model }
        );
        conn.Close();

        return result;
    }
    
    /// <summary>
    /// Returns the model by its unique key or null if not found.
    /// </summary>
    /// <param name="displayName"></param>
    /// <returns></returns>
    public static DiskInfo? FindByDisplayName(string displayName)
    {
        using var conn = DbConnection();
        conn.Open();
        
        var result = conn.QueryFirstOrDefault<DiskInfo>(
            @"SELECT * FROM DiskInfo WHERE DisplayName = @displayName",
            new { displayName }
        );
        conn.Close();

        return result;
    }
    
    /// <summary>
    /// Returns a list of all DiskInfo found in database.
    /// </summary>
    /// <returns>List</returns>
    public List<DiskInfo> FindAll()
    {
        using var conn = DbConnection();
        conn.Open();
        
        var itemsFound = conn.Query<DiskInfo>(@"SELECT * FROM DiskInfo").ToList();
        conn.Close();
        
        return itemsFound;
    }
    
    /// <summary>
    /// Save data to table. If UNIQUE serialNo is already exist in database, the Id will be null.
    /// </summary>
    /// <param name="diskInfo"></param>
    public void Save(DiskInfo diskInfo)
    {
        using var conn = DbConnection();
        conn.Open();
            
        // select query is for setting the Id of the model
        diskInfo.Id = conn.Query<long>(
            @"INSERT OR IGNORE INTO DiskInfo 
                    ( InterfaceType, MediaType, Model, DisplayName, SerialNo, CreatedAt ) VALUES 
                    ( @InterfaceType, @MediaType, @Model, @DisplayName, @SerialNo, @CreatedAt );
                    SELECT last_insert_rowid()", diskInfo
            ).First();
        
        conn.Close();
    }
    
    /// <summary>
    /// Delete DiskInfo (volume) by its Id.
    /// Make sure, you have cleared the dependant data by
    /// using i.e. "DeleteByDiskInfo(int diskInfoId)". 
    /// </summary>
    /// <param name="id"></param>
    public void DeleteById(long id)
    {
        using var conn = DbConnection();
        conn.Open();
            
        conn.Execute(@"DELETE FROM DiskInfo WHERE Id = @Id", new { id });
        
        conn.Close();
    }

    /// <summary>
    /// Creates table if not existing. Primary key is autoimcremented while serialNo is UNIQUE
    /// </summary>
    public sealed override void CreateTable()
    {
        const string sql = @"CREATE TABLE IF NOT EXISTS DiskInfo
                      (
                         Id                             INTEGER PRIMARY KEY NOT NULL, 
                         SerialNo                       TEXT NOT NULL UNIQUE,
                         DisplayName                    TEXT NOT NULL UNIQUE,
                         InterfaceType                  TEXT,
                         MediaType                      TEXT,
                         Model                          TEXT NOT NULL,
                         CreatedAt                      TEXT NOT NULL
                      )";

        ExecDbCmd(sql);
    }
   
    /// <summary>
    /// Deletes table with all data content.
    /// </summary>
    public override void DropTable()
    {
        ExecDbCmd(@"DROP TABLE IF EXISTS DiskInfo");
    }
}