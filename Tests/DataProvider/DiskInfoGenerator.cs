using PhotoTufin.Model;

namespace Tests.DataProvider;

public static class DiskInfoGenerator
{
    private static readonly string[] interFaces = { "SCSI", "USB" };
    private static readonly string[] mediaTypes = { "Fixed hard disk media", "Removable media", "Removable hard disk media" };
    private static readonly string[] models = { "WD_BLACK SN750 SE 1TB", "SanDisk Ultra Fit USB Device" };

    /// <summary>
    /// Generates a list of random DiskInfos.
    /// </summary>
    /// <param name="count"></param>
    /// <returns>List</returns>
    public static List<DiskInfo> generate(int count = 1)
    {
       var list = new List<DiskInfo>();
       var rand = new Random(); 
       for (var i = 0; i < count; i++)
       {
           var interfacesIndex = rand.Next(interFaces.Length-1);
           var mediaTypesIndex = rand.Next(mediaTypes.Length-1);
           var modelsIndex = rand.Next(mediaTypes.Length-1);
           
           var diskInfo = new DiskInfo()
           {
               InterfaceType = interFaces[interfacesIndex],
               MediaType = mediaTypes[mediaTypesIndex],
               Model = models[modelsIndex],
               SerialNo = Guid.NewGuid().ToString()
           };
           
           list.Add(diskInfo);
       }
       return list;
    }

    /// <summary>
    /// Picks a random element of a list.
    /// </summary>
    /// <param name="list"></param>
    /// <returns>DiskInfo</returns>
    /// <exception cref="ArgumentException"></exception>
    public static DiskInfo pickRandom(List<DiskInfo> list)
    {
        if (list.Count == 0)
            throw new ArgumentException("The provided list must contain at least one element");
        
        var rand = new Random();
        var index = rand.Next(list.Count);
        
        return list[index];
    }
}