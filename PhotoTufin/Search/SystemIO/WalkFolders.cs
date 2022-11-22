using System;
using System.Collections.Generic;
using System.IO;
using NLog;

namespace PhotoTufin.Search.SystemIO;

/// <summary>
/// Recursive walking down all subdirectories to find images by its extension.
/// </summary>
public class WalkFolders
{
    private readonly List<string> _extensionStrings;
    private readonly DirectoryInfo _searchStart;
    private static readonly Logger log = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="path"></param>
    /// <param name="extensionStrings"></param>
    public WalkFolders(string path, List<string> extensionStrings)
    {
        _extensionStrings = extensionStrings;
        _searchStart = new DirectoryInfo(path);
    }

    private int Count { get; set; }
    private List<ImageInfo> Files { get; } = new();

    /// <summary>
    /// Start search and collect file infos.
    /// </summary>
    /// <returns>List</returns>
    public List<ImageInfo> search()
    {
        Files.Clear();
        Count = 0; //init
        walkFolders(_searchStart);

        return Files;
    }
    
    /// <summary>
    /// Recursive call for subdirecxtories
    /// </summary>
    /// <param name="dirInfo"></param>
    private void walkFolders(DirectoryInfo dirInfo)
    {
        try
        {
            // recursion on all directories
            foreach (var subdirectories in dirInfo.GetDirectories())
            {
                walkFolders(subdirectories);
            }

            // reading file Info
            walkFiles(dirInfo);
        }
        catch (Exception e)
        {
            log.Error(e);
        }
    }

    /// <summary>
    /// Collect file info if extension is matching the filter (GUI).
    /// </summary>
    /// <param name="dirInfo"></param>
    private void walkFiles(DirectoryInfo dirInfo)
    {
        try
        {
            // walk thru all files
            foreach (var fileInfo in dirInfo.GetFiles())
            {
                const StringComparison ignoreCase = StringComparison.OrdinalIgnoreCase;
                if (_extensionStrings.FindIndex(x => x.Equals(fileInfo.Extension, ignoreCase)) == -1) continue;
            
            
                Count++; // use for debugging
                Files.Add(new ImageInfo(fileInfo));
            }
        }
        catch (Exception e)
        {
            log.Error(e);
        }
    }
}