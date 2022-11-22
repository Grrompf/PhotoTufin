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

    public static DiskInfo? GetDiskInfoByDisplayName(string? displayName)
    {
        return displayName == null ? null : DiskInfoRepository.FindByDisplayName(displayName);
    }
    
    /// <summary>
    /// Remove all disk related data from the database by its unique display name
    /// </summary>
    /// <param name="displayName"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Creates a unique display name by adding an ascnending number
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Saves a diskInfo as a scan result.
    /// </summary>
    /// <param name="selectedPath"></param>
    /// <returns></returns>
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