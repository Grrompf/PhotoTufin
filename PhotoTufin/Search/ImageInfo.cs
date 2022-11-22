using System;
using System.IO;
using NLog;
using PhotoTufin.Search.Duplication;

namespace PhotoTufin.Search;

/// <summary>
/// This is a transfer data model for photos. This is not an entity!
/// </summary>
public class ImageInfo
{
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="fileInfo"></param>
    public ImageInfo(FileInfo fileInfo)
    {
        FileName = fileInfo.Name;
        FilePath = EvalFilePath(fileInfo.FullName);
        Size = (fileInfo.Length / 1024).ToString();
        var hash = HashContent.readHash(fileInfo.FullName);
        HashString = HashContent.convertHash(hash);
    }
    
    /// <summary>
    /// Just the file name
    /// </summary>
    public string FileName { get; }
    
    /// <summary>
    /// Full file path without volume or netshare
    /// </summary>
    public string FilePath { get; }

    /// <summary>
    /// Filesize in kiloByte
    /// </summary>
    public string Size { get; }
    
    /// <summary>
    /// Hash of the file. Used to find duplicates. 
    /// </summary>
    public string? HashString { get; }

    /// <summary>
    /// Removes the volume of the full path file name.
    /// </summary>
    /// <param name="fullName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    private static string EvalFilePath(string fullName)
    {
        try
        {
            var rootPath = Path.GetPathRoot(fullName);
            if (rootPath == null) 
                throw new ArgumentNullException(rootPath);
            
            return fullName[rootPath.Length..];
        }
        catch (Exception e)
        {
            log.Error(e);
        }
        
        return Path.DirectorySeparatorChar.ToString();
    }
}