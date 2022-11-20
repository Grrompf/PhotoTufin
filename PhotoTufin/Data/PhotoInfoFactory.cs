using System.Collections.Generic;
using System.Runtime.Versioning;
using PhotoTufin.Model;
using PhotoTufin.Repository;
using PhotoTufin.Search;

namespace PhotoTufin.Data;

[SupportedOSPlatform("windows")]
public static class PhotoInfoFactory
{
    private static PhotoInfoRepository PhotoInfoRepository { get; } = new();

    public static List<PhotoInfo> GetDuplicatesByDiskInfo(string? displayName)
    {
        var emptyList = new List<PhotoInfo>();
        if (displayName == null)
            return emptyList;
        
        var diskInfo = DiskInfoRepository.FindByDisplayName(displayName);
        return diskInfo == null ? emptyList : PhotoInfoRepository.FindDuplicatesByDiskInfo(diskInfo.Id);
    }
    
    public static int GetImageCount(string? displayName)
    {
        if (displayName == null)
            return 0;
        
        var diskInfo = DiskInfoRepository.FindByDisplayName(displayName);
        return diskInfo == null ? 0 : PhotoInfoRepository.GetImageCount(diskInfo.Id);
    }
    
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