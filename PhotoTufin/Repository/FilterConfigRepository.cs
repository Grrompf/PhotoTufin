using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using PhotoTufin.Model;

namespace PhotoTufin.Repository;

/// <summary>
/// Repository of filter configuration
/// </summary>
public class FilterConfigRepository : BaseRepository
{
    /// <summary>
    /// By Using the constructor, database and table will be created if not existing.  
    /// </summary>
    public FilterConfigRepository()
    {
        CreateTable();
    }
    
    /// <summary>
    /// Returns the model by its unique key or null if not found.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns>FilterConfig?</returns>
    public FilterConfig? FindByFilter(string filter)
    {
        try
        {
            using var conn = DbConnection();
            conn.Open();
        
            var result = conn.QueryFirstOrDefault<FilterConfig>(
                @"SELECT * FROM FilterConfig WHERE Filter = @filter",
                new { filter }
            );
            conn.Close();

            return result;

        }
        catch (Exception e)
        {
            log.Error(e);
        }

        return null;
    }
    
    /// <summary>
    /// Returns a list of all FilterConfig found in database.
    /// </summary>
    /// <returns>List</returns>
    public List<FilterConfig> FindAll()
    {
        try
        {
            using var conn = DbConnection();
            conn.Open();
        
            var itemsFound = conn.Query<FilterConfig>(@"SELECT * FROM FilterConfig").ToList();
            conn.Close();
        
            return itemsFound;
        }
        catch (Exception e)
        {
            log.Error(e);
        }

        return new List<FilterConfig>();
    }
    
    /// <summary>
    /// Save data to table. If UNIQUE Filter does already exist in database,
    /// the Id will be null.
    /// </summary>
    /// <param name="filterConfig"></param>
    public void Save(FilterConfig filterConfig)
    {
        try
        {
            using var conn = DbConnection();
            conn.Open();
            
            // select query is for setting the Id of the model
            filterConfig.Id = conn.Query<long>(
                @"INSERT OR IGNORE INTO FilterConfig 
                    ( Filter, IsChecked, CreatedAt ) VALUES 
                    ( @Filter, @IsChecked, @CreatedAt );
                    SELECT last_insert_rowid()", filterConfig
            ).First();
        
            conn.Close();
        }
        catch (Exception e)
        {
            log.Error(e);
        }
        
    }
    
    /// <summary>
    /// Creates table if not existing. Primary key is autoincremented.
    /// Filter is UNIQUE
    /// </summary>
    public sealed override void CreateTable()
    {
        try
        {
            const string sql = @"CREATE TABLE IF NOT EXISTS FilterConfig
                      (
                         Id                             INTEGER PRIMARY KEY NOT NULL, 
                         Filter                         TEXT NOT NULL UNIQUE,
                         IsChecked                      INTEGER,
                         CreatedAt                      TEXT NOT NULL
                      )";

            ExecDbCmd(sql);
        }
        catch (Exception e)
        {
            log.Error(e);
        }
        
    }
   
    /// <summary>
    /// Deletes table with all data content.
    /// </summary>
    public override void DropTable()
    {
        try
        {
            ExecDbCmd(@"DROP TABLE IF EXISTS FilterConfig");
        }
        catch (Exception e)
        {
            log.Error(e);
        }
        
    }
}