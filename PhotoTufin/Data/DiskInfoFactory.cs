using System.Collections.Generic;
using System.Runtime.Versioning;
using PhotoTufin.Model;
using PhotoTufin.Repository;
using PhotoTufin.Search.SystemIO;

namespace PhotoTufin.Data;

[SupportedOSPlatform("windows")]
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

    private static string CreateUniqueDisplayName(string model)
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

    public static DiskInfo? SaveDiskInfo(string selectedPath)
    {
        var reader = new HDDInfoReader(selectedPath);
        
        // get wmi infos of the hdd
        var hddInfo = reader.GetDiskInfo();
        if (hddInfo?.SerialNo == null)
            return null;

        var repository = new DiskInfoRepository();
        var diskInfo = repository.FindBySerialNo(hddInfo.SerialNo);
        
        // diskInfo already exist
        if (diskInfo != null)
            return diskInfo;
        
        var displayName = CreateUniqueDisplayName(hddInfo.Model);
        
        diskInfo = new DiskInfo
        {
            DisplayName = displayName,
            Model = hddInfo.Model,
            InterfaceType = hddInfo.InterfaceType,
            MediaType = hddInfo.MediaType,
            SerialNo = hddInfo.SerialNo
        };
        repository.Save(diskInfo);
        
        return diskInfo;
    }
}