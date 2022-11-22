using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Windows.Controls;
using PhotoTufin.Search.Duplication;
using PhotoTufin.Search.Filter;
using PhotoTufin.Search.SystemIO;

namespace PhotoTufin.Search;

[SupportedOSPlatform("windows")]
public static class ScanFactory
{
    /// <summary>
    /// Duplicates on itself
    /// </summary>
    public static int NoDuplicates { get; private set; }
    
    /// <summary>
    /// Finds all images by walking all folders by its extension (filter). 
    /// </summary>
    /// <param name="selectedPath"></param>
    /// <param name="filter"></param>
    /// <returns>List</returns>
    public static List<ImageInfo> FindImages(string selectedPath, MenuItem filter)
    {
        var fileExtensions = ImageFilter.makeFilter(filter);
        var search = new WalkFolders(selectedPath, fileExtensions);
        var imageInfos = search.search();

        NoDuplicates = DuplicateFinder.findDuplicates(ref imageInfos);

        return imageInfos;
    }
}