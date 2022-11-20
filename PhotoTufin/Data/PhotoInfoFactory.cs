using System.Collections.Generic;
using PhotoTufin.Model;
using PhotoTufin.Repository;

namespace PhotoTufin.Data;

public static class PhotoInfoFactory
{
    private static DiskInfoRepository DiskInfoRepository { get; } = new();
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
}