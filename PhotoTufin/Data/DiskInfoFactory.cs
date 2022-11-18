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
        return displayName == null ? null : Repository.FindByDisplayName(displayName);
    }
    
}