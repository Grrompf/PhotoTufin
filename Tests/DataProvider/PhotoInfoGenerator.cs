using PhotoTufin.Model;

namespace Tests.DataProvider;

public static class PhotoInfoGenerator
{
    private static readonly string[] pathStrings = { "MyPics\\Vacation\\", "MyPics\\Secret\\BlackMail", "OldPics\\Selfies", "New\\Covers\\Vogue" };

    /// <summary>
    /// Generates a list of random PhotoInfos.
    /// </summary>
    /// <param name="diskInfo"></param>
    /// <param name="count"></param>
    /// <returns>List</returns>
    public static List<PhotoInfo> generate(DiskInfo diskInfo, int count = 1)
    {
       var list = new List<PhotoInfo>();
       var rand = new Random(); 
       for (var i = 0; i < count; i++)
       {
           var fileName = $"{Guid.NewGuid().ToString()}.jpg";
           var pathIndex = rand.Next(pathStrings.Length-1);
           var hashString = Guid.NewGuid().ToString();
           hashString = hashString.Replace("-", string.Empty);
           
           var photoInfo = new PhotoInfo()
           {
               DiskInfoId = diskInfo.Id,
               FileName = fileName,
               FilePath = $"{pathStrings[pathIndex]}\\{fileName}",
               Size = rand.Next(150, 2000).ToString(),
               HashString = hashString
           };
           
           list.Add(photoInfo);
       }
       return list;
    }
    
    /// <summary>
    /// Generates a list of random PhotoInfos with duplicates.
    /// </summary>
    /// <param name="diskInfo"></param>
    /// <param name="hashString"></param>
    /// <param name="count"></param>
    /// <returns>List</returns>
    public static List<PhotoInfo> generateDuplicates(DiskInfo diskInfo, string hashString, int count = 1)
    {
        var list = new List<PhotoInfo>();
        var rand = new Random(); 
        for (var i = 0; i < count; i++)
        {
            var fileName = $"{Guid.NewGuid().ToString()}.jpg";
            var pathIndex = rand.Next(pathStrings.Length-1);
            
            var photoInfo = new PhotoInfo()
            {
                DiskInfoId = diskInfo.Id,
                FileName = fileName,
                FilePath = $"{pathStrings[pathIndex]}\\{fileName}",
                Size = rand.Next(150, 2000).ToString(),
                HashString = hashString
            };
           
            list.Add(photoInfo);
        }
        return list;
    }

    /// <summary>
    /// Picks a random element of a list.
    /// </summary>
    /// <param name="list"></param>
    /// <returns>PhotoInfo</returns>
    /// <exception cref="ArgumentException"></exception>
    public static PhotoInfo pickRandom(List<PhotoInfo> list)
    {
        if (list.Count == 0)
            throw new ArgumentException("The provided list must contain at least one element");
        
        var rand = new Random();
        var index = rand.Next(list.Count);
        
        return list[index];
    }
}