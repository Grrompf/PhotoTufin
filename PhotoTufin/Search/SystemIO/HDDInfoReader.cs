using System;
using System.Management;

namespace PhotoTufin.Search.SystemIO;

public class HDDInfoReader
{
    /// <summary>
    /// get serial number of HDD by partition insert
    /// </summary>
    /// <param name="partition">partition name</param>
    /// <returns></returns>
    public static string? GetSerialNumber(string partition)
    {
        if (partition.Length != 2) return null;
        
        var model = GetHddInfoFromPartition(partition);
        Console.WriteLine($"{model?.InterfaceType}{model?.Model}{model?.MediaType}{model?.SerialNo}");

        return model?.SerialNo;
        //return GetHDDSerial(model);
    }
    
    /// <summary>
    /// get HDD info from partition name
    /// </summary>
    /// <param name="partition"></param>
    /// <returns></returns>
    public static HDDInfo? GetHddInfoFromPartition(string partition)
    {
        // validation eg d: 
        if (partition.Length != 2) return null;
        
        try
        {
            var query = $"ASSOCIATORS OF {{Win32_LogicalDisk.DeviceID='{partition}'}} WHERE ResultClass=Win32_DiskPartition";
            using var par = new ManagementObjectSearcher(query);
            
            foreach (var p in par.Get())
            {
                return getWmiHdd(p);
            }
        }
        catch (Exception)
        {
            // ignored
        }

        return null;
    }

    private static HDDInfo? getWmiHdd(ManagementBaseObject mngObject)
    {
        var query = $"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{mngObject["DeviceID"]}'}} WHERE ResultClass=Win32_DiskDrive";
        using var drive = new ManagementObjectSearcher(query);
        
        foreach (var driveObject in drive.Get())
        {
            var wmi_HDD = new HDDInfo
            {
                MediaType = driveObject["MediaType"].ToString(),
                Model = driveObject["Model"].ToString(),
                SerialNo = driveObject["SerialNumber"].ToString(),
                InterfaceType = driveObject["InterfaceType"].ToString()
            };
            return wmi_HDD;
        }
        
        return null;
    }
}