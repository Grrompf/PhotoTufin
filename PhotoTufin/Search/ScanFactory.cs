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
    public static int NoDuplicates { get; private set; }
    
    public static List<ImageInfo> FindImages(string selectedPath, MenuItem filter)
    {
        var fileExtensions = ImageFilter.makeFilter(filter);
        var search = new WalkFolders(selectedPath, fileExtensions);
        var imageInfos = search.search();

        NoDuplicates = DuplicateFinder.findDuplicates(ref imageInfos);

        //SaveAsync(imageInfos, diskInfo);

        return imageInfos;
    }
}