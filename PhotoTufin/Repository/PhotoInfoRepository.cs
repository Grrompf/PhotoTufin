using System.Collections.Generic;
using System.Linq;
using Dapper;
using PhotoTufin.Model;

namespace PhotoTufin.Repository;

internal class PhotoInfoRepository : BaseRepository, IPhotoInfoRepository
{
    /// <summary>
    /// By Using the constructor, database and table will be created if not existing.  
    /// </summary>
    public PhotoInfoRepository()
    {
        CreateTable();
    }
    
    /// <summary>
    /// Returns the model by its primary key or null if not found.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>PhotoInfo?</returns>
    public PhotoInfo? Find(long id)
    {
        using var conn = DbConnection();
        conn.Open();
        
        var result = conn.QueryFirstOrDefault<PhotoInfo>(
            @"SELECT * FROM PhotoInfo WHERE Id = @Id",
            new { id }
        );
        conn.Close();

        return result;
    }
    
    /// <summary>
    /// Returns a list of all duplicates in database.
    /// </summary>
    /// <returns>List</returns>
    public List<PhotoInfo> FindDuplicates()
    {
        using var conn = DbConnection();
        conn.Open();
        
        // we have to make an inner request for duplicates by gruop and having. 
        // a count > 1 means there a more than just one - so it is a duplicate
        var result = conn.Query<PhotoInfo>(
            @"SELECT * FROM PhotoInfo WHERE HashString IN 
                 (SELECT HashString FROM PhotoInfo GROUP BY HashString HAVING COUNT(HashString) > 1)"
            ).ToList();
        
        conn.Close();

        return result;
    }
    
    /// <summary>
    /// Find alle duplicates of a volume (DiskInfo) of a HDD or removable media.
    /// </summary>
    /// <param name="diskInfoId"></param>
    /// <returns>List</returns>
    public List<PhotoInfo> FindDuplicatesByDiskInfo(int diskInfoId)
    {
        using var conn = DbConnection();
        conn.Open();
        
        var result = conn.Query<PhotoInfo>(
            @"SELECT * FROM PhotoInfo WHERE DiskInfoId = @DiskInfoId AND HashString IN 
                 (SELECT HashString FROM PhotoInfo GROUP BY HashString HAVING COUNT(HashString) > 1)",
            new {diskInfoId}
            ).ToList();
        
        conn.Close();

        return result;
    }
    
    /// <summary>
    /// Find alle duplicates by its hash and inner request for having a duplicate.
    /// </summary>
    /// <param name="hashString"></param>
    /// <returns>List</returns>
    public List<PhotoInfo> FindDuplicatesByHashString(string hashString)
    {
        using var conn = DbConnection();
        conn.Open();
        
        var result = conn.Query<PhotoInfo>(
            @"SELECT * FROM PhotoInfo WHERE HashString = @HashString AND HashString IN 
                 (SELECT HashString FROM PhotoInfo GROUP BY HashString HAVING COUNT(HashString) > 1)",
            new {hashString}
        ).ToList();
        
        conn.Close();

        return result;
    }
    
    /// <summary>
    /// Returns a list of all DiskInfo found in database.
    /// </summary>
    /// <returns>List</returns>
    public List<PhotoInfo> FindAll()
    {
        using var conn = DbConnection();
        conn.Open();
        
        var itemsFound = conn.Query<PhotoInfo>(@"SELECT * FROM PhotoInfo").ToList();
        conn.Close();
        
        return itemsFound;
    }
    
    /// <summary>
    /// Save data to table. If UNIQUE serialNo is already exist in database, the Id will be null.
    /// </summary>
    /// <param name="photoInfo"></param>
    public void Save(PhotoInfo photoInfo)
    {
        using var conn = DbConnection();
        conn.Open();
            
        // select query is for setting the Id of the model
        photoInfo.Id = conn.Query<long>(
            @"INSERT OR IGNORE INTO PhotoInfo 
                    ( DiskInfoId, FileName, FilePath, Size, HashString, Tuplet, CreatedAt ) VALUES 
                    ( @DiskInfoId, @FileName, @FilePath, @Size, @HashString, @Tuplet, @CreatedAt );
                    SELECT last_insert_rowid()", photoInfo
            ).First();
        
        conn.Close();
    }

    /// <summary>
    /// Deletes all PhotoInfo of a DiskInfo (volume).   
    /// </summary>
    /// <param name="diskInfoId"></param>
    public void DeleteByDiskInfo(int diskInfoId)
    {
        using var conn = DbConnection();
        conn.Open();
            
        conn.Execute(@"DELETE FROM PhotoInfo WHERE DiskInfoId = @DiskInfoId", new { diskInfoId });
        
        conn.Close();
    }

    /// <summary>
    /// Creates table if not existing. Primary key is autoimcremented while serialNo is UNIQUE
    /// </summary>
    public sealed override void CreateTable()
    {
        const string sql = @"CREATE TABLE IF NOT EXISTS PhotoInfo
                      (
                         Id                             INTEGER PRIMARY KEY NOT NULL, 
                         DiskInfoId                     INTEGER,
                         FileName                       TEXT NOT NULL,
                         FilePath                       TEXT NOT NULL,
                         Size                           TEXT,
                         HashString                     TEXT NOT NULL,
                         Tuplet                         INTEGER,
                         CreatedAt                      TEXT NOT NULL,                         
                         CONSTRAINT fk_diskInfo
                            FOREIGN KEY (Id)
                            REFERENCES DiskInfo(Id)
                      )";

        ExecDbCmd(sql);
    }
   
    /// <summary>
    /// Deletes table with all data content.
    /// </summary>
    public override void DropTable()
    {
        ExecDbCmd(@"DROP TABLE IF EXISTS PhotoInfo");
    }
}