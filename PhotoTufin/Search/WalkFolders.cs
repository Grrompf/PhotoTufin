using System;
using System.Collections.Generic;
using System.IO;

namespace PhotoTufin.Search;

public class WalkFolders
{
    private readonly List<string> _extensionStrings;
    private readonly DirectoryInfo _searchStart;

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
            Console.WriteLine(e.Message);
        }
    }

    /// <summary>
    /// Collect file info if extension is matching the filter (GUI).
    /// </summary>
    /// <param name="dirInfo"></param>
    private void walkFiles(DirectoryInfo dirInfo)
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
}