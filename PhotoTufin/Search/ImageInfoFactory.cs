using System.Collections.Generic;
using System.Windows.Controls;
using PhotoTufin.Search.Duplication;
using PhotoTufin.Search.Filter;
using PhotoTufin.Search.SystemIO;

namespace PhotoTufin.Search;

public class TupletFactory
{
    public TupletFactory(string selectedPath, MenuItem filter)
    {
        SelectedPath = selectedPath;
        Filter = filter;
    }
    
    public int NoDuplicates { get; private set; }

    public string SelectedPath { get; }

    public MenuItem Filter { get; }

    public List<ImageInfo> findImages()
    {
        var fileExtensions = ImageFilter.makeFilter(Filter);
        var search = new WalkFolders(SelectedPath, fileExtensions);
        var imageInfos = search.search();

        NoDuplicates = DuplicateFinder.findDuplicates(ref imageInfos);

        return imageInfos;
    }
}