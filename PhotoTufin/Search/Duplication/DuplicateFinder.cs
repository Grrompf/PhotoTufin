using System.Collections.Generic;
using System.Linq;

namespace PhotoTufin.Search.Duplication;

public static class DuplicateFinder
{
    public static int findDuplicates(ref List<ImageInfo> imageInfos)
    {
        var dupes = imageInfos.GroupBy(x => x.hashContent)
            .Where(g => g.Count() > 1)
            .SelectMany(g => g)
            ;

        var duplicates = dupes.ToList();
        
        indicateDuplicates(ref imageInfos, duplicates);

        return duplicates.Count;
    }

    private static void indicateDuplicates(ref List<ImageInfo> imageInfos, ICollection<ImageInfo> duplicates)
    {
        foreach (var t in imageInfos.Where(duplicates.Contains))
        {
            t.tuplet = true;
        }
    }
}