using System.Collections.Generic;
using System.Linq;

namespace PhotoTufin.Search.Duplication;

/// <summary>
/// Used to identify duplicates on a scan by itself. Probably obsolete since a database was implemented. 
/// </summary>
public static class DuplicateFinder
{
    /// <summary>
    /// Finds instant duplicates of a scan (list of ImageInfos). Is not using the database. 
    /// </summary>
    /// <param name="imageInfos"></param>
    /// <returns></returns>
    public static int findDuplicates(ref List<ImageInfo> imageInfos)
    {
        var dupes = imageInfos.GroupBy(x => x.HashString)
            .Where(g => g.Count() > 1)
            .SelectMany(g => g)
            ;

        var duplicates = dupes.ToList();
        
        return duplicates.Count;
    }
}