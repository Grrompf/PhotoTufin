namespace PhotoTufin.Model;

public interface IPhotoInfo
{
    /// <summary>
    /// Just the file name
    /// </summary>
    public string FileName { get; set; }
    
    /// <summary>
    /// Full file path without volume or netshare
    /// </summary>
    public string FilePath { get; set; }

    /// <summary>
    /// Filesize in kiloByte
    /// </summary>
    public string Size { get; set; }
    
    /// <summary>
    /// Hash of the file. Used to find duplicates. 
    /// </summary>
    public string HashString { get; set; }
}