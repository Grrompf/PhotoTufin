using System.Collections.Generic;
using System.Runtime.Versioning;
using PhotoTufin.Model;
using PhotoTufin.Repository;
using PhotoTufin.Search;

namespace PhotoTufin.Data;

/// <summary>
/// Easy access to photo related procedures on database. 
/// </summary>
[SupportedOSPlatform("windows")]
public static class PhotoInfoFactory
{
    private static PhotoInfoRepository PhotoInfoRepository { get; } = new();

    /// <summary>
    /// Get a list of duplicates by diskInfo. The duplicates are compared to all
    /// photos of the whole database 
    /// </summary>
    /// <param name="displayName"></param>
    /// <returns></returns>
    public static List<PhotoInfo> GetDuplicatesByDiskInfo(string? displayName)
    {
        var emptyList = new List<PhotoInfo>();
        if (displayName == null)
            return emptyList;
        
        var diskInfo = DiskInfoRepository.FindByDisplayName(displayName);
        return diskInfo == null ? emptyList : PhotoInfoRepository.FindDuplicatesByDiskInfo(diskInfo.Id);
    }
    
    /// <summary>
    /// Get the number of images by diskInfo 
    /// </summary>
    /// <param name="displayName"></param>
    /// <returns></returns>
    public static int GetImageCount(string? displayName)
    {
        if (displayName == null)
            return 0;
        
        var diskInfo = DiskInfoRepository.FindByDisplayName(displayName);
        return diskInfo == null ? 0 : PhotoInfoRepository.GetImageCount(diskInfo.Id);
    }
    
    /// <summary>
    /// Save list scan of photos into the database
    /// </summary>
    /// <param name="imageInfos"></param>
    /// <param name="diskInfo"></param>
    public static void SavePhotos(List<ImageInfo> imageInfos, IModel? diskInfo)
    {
        if (diskInfo == null) return;
        foreach (var image in imageInfos)
        {
            if (image.HashString == null)
                continue;
            
            var photo = new PhotoInfo
            {
                DiskInfoId = diskInfo.Id,
                HashString = image.HashString,
                FileName = image.FileName,
                FilePath = image.FilePath,
                Size = image.Size
            };
            PhotoInfoRepository.Save(photo);
        }
    }
}