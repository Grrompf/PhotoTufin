using System;

namespace PhotoTufin.Model;

public class PhotoInfo : IModel, IPhotoInfo
{
    /// <summary>
    /// Primary Key 
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// FK constraint to DiskInfo primary key Id. 
    /// </summary>
    public long DiskInfoId { get; set; }
    
    /// <summary>
    /// Inner Join. 
    /// </summary>
    public DiskInfo? DiskInfo { get; set; }

    /// <summary>
    /// Helper method for showing the displayName since dapper cannot provide the innerJoin properly.
    /// This is not part of the entity.
    /// </summary>
    public string DisplayName => DiskInfo == null ? "Unknown" : DiskInfo.DisplayName;

    /// <summary>
    /// Just the file name
    /// </summary>
    public string FileName { get; set; } = null!;
    
    /// <summary>
    /// Full file path without volume or netshare
    /// </summary>
    public string FilePath { get; set; } = null!;

    /// <summary>
    /// Filesize in kiloByte
    /// </summary>
    public string Size { get; set; } = null!;
    
    /// <summary>
    /// Hash of the file. Used to find duplicates. 
    /// </summary>
    public string HashString { get; set; } = null!;

    /// <summary>
    /// If TRUE this image has duplicates
    /// </summary>
    public bool Tuplet { get; set; }
    
    /// <summary>
    /// when this model was created 
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}