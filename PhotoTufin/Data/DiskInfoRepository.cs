using System.Collections.Generic;
using System.Linq;
using Dapper;
using PhotoTufin.Model;

namespace PhotoTufin.Data;

internal class DiskInfoRepository : BaseRepository, IDiskInfoRepository
{
    public DiskInfo? Find(long id)
    {
        using var conn = DbConnection();
        conn.Open();
        
        return conn.QueryFirstOrDefault<DiskInfo>(@"SELECT * FROM DiskInfo WHERE Id = @id", new { id });
    }
    
    public List<DiskInfo> FindAll()
    {
        using var conn = DbConnection();
        conn.Open();
        
        return conn.Query<DiskInfo>(@"SELECT * FROM DiskInfo").ToList();
    }
    
    public void Save(DiskInfo diskInfo)
    {
        using var conn = DbConnection();
        conn.Open();
        
        // select query is for setting the Id of the model
        diskInfo.ID = conn.Query<long>(
            sql: @"INSERT INTO DiskInfo 
                    ( InterfaceType, MediaType, Model, SerialNo, CreatedAt ) VALUES 
                    ( @InterfaceType, @MediaType, @Model, @SerialNo, @CreatedAt );
                    SELECT last_insert_rowid()", diskInfo).First();
    }

    public override string CreateTableSQL()
    {
        return @"CREATE TABLE IF NOT EXISTS DiskInfo
                      (
                         ID                             INTEGER NOT NULL PRIMARY KEY,
                         InterfaceType                  TEXT,
                         MediaType                      TEXT,
                         Model                          TEXT NOT NULL,
                         SerialNo                       TEXT NOT NULL UNIQUE,
                         CreatedAt                      TEXT NOT NULL
                      );
                ";
    }
    
    public override string DropTableSQL()
    {
        return @"DROP TABLE IF EXISTS DiskInfo";
    }
}