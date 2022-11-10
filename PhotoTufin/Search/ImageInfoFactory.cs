using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Windows.Controls;
using PhotoTufin.Search.Duplication;
using PhotoTufin.Search.Filter;
using PhotoTufin.Search.SystemIO;

namespace PhotoTufin.Search;

[SupportedOSPlatform("windows")]
public class ImageInfoFactory
{
    public ImageInfoFactory(string selectedPath, MenuItem filter)
    {
        SelectedPath = selectedPath;
        Filter = filter;
        DiskReader = new HDDInfoReader(selectedPath);
    }
    
    public int NoDuplicates { get; private set; }
    
    public HDDInfo? HDDInfo { get; set;  }

    private string SelectedPath { get; }

    private HDDInfoReader DiskReader { get; }
    
    private MenuItem Filter { get; }
    
    public List<ImageInfo> findImages()
    {
        // get wmi infos of the hdd
        HDDInfo = DiskReader.GetDiskInfo();
        
        var fileExtensions = ImageFilter.makeFilter(Filter);
        var search = new WalkFolders(SelectedPath, fileExtensions);
        var imageInfos = search.search();

        NoDuplicates = DuplicateFinder.findDuplicates(ref imageInfos);

        return imageInfos;
    }
}