using System;
using System.IO;
using System.Management;
using System.Runtime.Versioning;

namespace PhotoTufin.Search.SystemIO;

/// <summary>
/// Get the system information of the data storage drive you nare currently scanning for files.
/// This uses Windows-specific libraries making this windows-only. 
/// </summary>
[SupportedOSPlatform("windows")]
public class HDDInfoReader
{
    private string FullPath { get;}
    
    /// <summary>
    ///  Constructor.
    /// </summary>
    /// <param name="selectedPath">Full path of the directory e.g. C:/mySpace</param>
    public HDDInfoReader(string selectedPath)
    {
        FullPath = selectedPath;
    }
    
    /// <summary>
    /// Get WMI information of the HDD e.g. serial number, model, interface type and media type. 
    /// </summary>
    /// <exception cref="Exception"></exception>
    public HDDInfo? GetDiskInfo()
    {
        try
        {
            var fileInfo = new FileInfo(FullPath);
            var rootPath = Path.GetPathRoot(fileInfo.FullName);
        
            // now we have just the drive letter and the volume seperator e.g. "D:"
            var partition = rootPath?.Replace(Path.DirectorySeparatorChar.ToString(), "");

            if (partition == null) 
                throw new ArgumentNullException(partition);

            return GetHddInfoByPartition(partition);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
    }
    
    /// <summary>
    /// Get HDD infos by volume (partition).
    /// </summary>
    /// <param name="partition"></param>
    /// <returns>bool</returns>
    /// <exception cref="Exception"></exception>
    private static HDDInfo? GetHddInfoByPartition(string partition)
    {
        try
        {
            var query = $"ASSOCIATORS OF {{Win32_LogicalDisk.DeviceID='{partition}'}} WHERE ResultClass=Win32_DiskPartition";
            using var diskPartitions = new ManagementObjectSearcher(query);
            
            foreach (var volume in diskPartitions.Get())
            {
                return GetWmiHdd(volume);
            }

            return null;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    /// <summary>
    /// Get internal information of the HDD by WMI (Microsoft)
    /// </summary>
    /// <param name="mngObject"></param>
    /// <exception cref="Exception"></exception>
    private static HDDInfo? GetWmiHdd(ManagementBaseObject mngObject)
    {
        try
        {
            var query =
                $"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{mngObject["DeviceID"]}'}} WHERE ResultClass=Win32_DiskDrive";
            using var drive = new ManagementObjectSearcher(query);

            foreach (var driveObject in drive.Get())
            {
                if (
                    driveObject["Model"] == null ||
                    driveObject["SerialNumber"] == null ||
                    driveObject["InterfaceType"] == null ||
                    driveObject["MediaType"] == null
                ) 
                {
                    throw new Exception("Cannot read this disk drive");
                }

                var model = driveObject["Model"].ToString();
                var serialNo = driveObject["SerialNumber"].ToString();
                
                if (model == null || serialNo == null) 
                {
                    throw new Exception("Cannot access essential disk drive information");
                }
                var interfaceType = driveObject["InterfaceType"].ToString();
                var media = driveObject["MediaType"].ToString();

                return new HDDInfo(model, serialNo, interfaceType, media);
            }

            return null;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}