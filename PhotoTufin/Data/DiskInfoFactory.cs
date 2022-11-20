using System.Collections.Generic;
using PhotoTufin.Model;
using PhotoTufin.Repository;
using PhotoTufin.Search.SystemIO;

namespace PhotoTufin.Data;

public static class DiskInfoFactory
{
    private static DiskInfoRepository Repository { get; } = new();

    public static List<DiskInfo> GetAllDisks()
    {
        return Repository.FindAll();
    }

    public static DiskInfo? GetDiskByScanResult(HDDInfo? info)
    {
        return info == null ? null : Repository.FindBySerialNo(info.SerialNo);
    }

    public static DiskInfo? GetDiskInfoByDisplayName(string? displayName)
    {
        return displayName == null ? null : DiskInfoRepository.FindByDisplayName(displayName);
    }
    
    public static bool DeleteDiskAndPhotoData(string displayName)
    {
        var diskInfo = GetDiskInfoByDisplayName(displayName);
        if (diskInfo == null)
            return false;

        var photoInfoRepo = new PhotoInfoRepository();
        photoInfoRepo.DeleteByDiskInfo(diskInfo.Id);
            
        var diskInfoRepo = new DiskInfoRepository();
        diskInfoRepo.DeleteById(diskInfo.Id);

        return true;
    }

    public static string CreateUniqueDisplayName(string model)
    {
        var words = model.Split(' ');

        var displayName = "";
        foreach (var word in words)
        {
            displayName += $"{word} ";
            if (displayName.Length > 14)
                break;
        }
        
        // get next number
        var nextNumber = DiskInfoRepository.GetModelCount(model) + 1;
        displayName += $"- {nextNumber}";
        
        return displayName;
    }
}