using PhotoTufin.Model;

namespace Tests.DataProvider;

public static class FilterGenerator
{
    private static readonly string[] filters = { "bmp, dib", "gif", "crw, cr2, cr3", "png" };
    
    /// <summary>
    /// Generates a list of random DiskInfos.
    /// </summary>
    /// <param name="count"></param>
    /// <returns>List</returns>
    public static List<FilterConfig> generate(int count = 1)
    {
       var list = new List<FilterConfig>();
       var rand = new Random(); 
       for (var i = 0; i < count; i++)
       {
           var index = rand.Next(filters.Length-1);
           var isChecked = new Random().Next(100) <= 50;
           
           var diskInfo = new FilterConfig()
           {
               Filter = filters[index],
               IsChecked = isChecked
           };
           
           list.Add(diskInfo);
       }
       return list;
    }

    /// <summary>
    /// Picks a random element of a list.
    /// </summary>
    /// <param name="list"></param>
    /// <returns>FilterConfig</returns>
    /// <exception cref="ArgumentException"></exception>
    public static FilterConfig pickRandom(List<FilterConfig> list)
    {
        if (list.Count == 0)
            throw new ArgumentException("The provided list must contain at least one element");
        
        var rand = new Random();
        var index = rand.Next(list.Count);
        
        return list[index];
    }
}