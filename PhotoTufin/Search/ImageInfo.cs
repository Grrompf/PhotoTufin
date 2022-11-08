using System;
using System.IO;
using PhotoTufin.Search.Duplication;

namespace PhotoTufin.Search;

public class ImageInfo
{
    public ImageInfo(FileInfo fileInfo)
    {
        FileName = fileInfo.Name;
        FullPath = fileInfo.FullName;
        FilePath = EvalFilePath(fileInfo.FullName);
        Size = (fileInfo.Length / 1024).ToString();
        Date = fileInfo.CreationTime.ToShortDateString();
        Tuplet = false;
        Action = false;
        Hash = HashContent.readHash(fileInfo.FullName);
        HashString = HashContent.readHash(fileInfo.FullName)?.ToString();
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
    /// Full path to the file e.g. D:\myDir\img.jpg
    /// </summary>
    public string FullPath { get; }
    
    /// <summary>
    /// Filesize in kiloByte
    /// </summary>
    public string Size { get; }
    
    /// <summary>
    /// The creation date of the file by means when the file was copied to that full path position 
    /// </summary>
    public string Date { get; }
    public bool Tuplet { get; set; }
    public bool Action { get; }
    public byte[]? Hash { get; }
    
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
        catch (Exception)
        {
            return Path.DirectorySeparatorChar.ToString();
        }
    }
}