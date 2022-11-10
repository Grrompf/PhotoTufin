using System.IO;
using System.Linq;
using Dapper;
using PhotoTufin.Model;

namespace PhotoTufin.Data;

internal class DiskInfoRepository : BaseRepository
{
    public DiskInfo? GetDiskInfo(long id)
    {
        if (!File.Exists(DbFile)) return null;

        using var con = DbConnection();
        con.Open();
        
        return con.QueryFirstOrDefault<DiskInfo>(@"SELECT * FROM DiskInfo WHERE Id = @id", new { id });
    }

    public void SaveDiskInfo(DiskInfo diskInfo)
    {
        if (!File.Exists(DbFile)) CreateTable();

        using var cnn = DbConnection();
        cnn.Open();
        diskInfo.Id = cnn.Query<long>(
            @"INSERT INTO DiskInfo 
                    ( InterfaceType, MediaType, Model, SerialNo, CreatedAt ) VALUES 
                    ( @InterfaceType, @MediaType, @Model, @SerialNo, @CreatedAt );
                    
                select last_insert_rowid()", diskInfo).First();
    }

    public override string CreateTableSQL()
    {
        return @"CREATE TABLE IF NOT EXISTS DiskInfo
                      (
                         ID                              INTEGER NOT NULL PRIMARY KEY ,
                         InterfaceType                   TEXT DEFAULT 'UNKNOWN',
                         MediaType                       TEXT DEFAULT 'UNKNOWN',
                         Model                           TEXT NOT NULL,
                         SerialNo                        TEXT NOT NULL UNIQUE,
                         CreatedAt                       TEXT NOT NULL
                      );
                ";
    }
    
    public override string DropTableSQL()
    {
        return @"DROP TABLE IF EXISTS DiskInfo";
    }
}