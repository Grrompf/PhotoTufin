using System;

namespace PhotoTufin.Model;

/// <summary>
/// Entity model of hard disk infos.
/// </summary>
public class DiskInfo : IModel
{
    /// <summary>
    /// Primary key
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// eg E823_8FA6_BF53_0001_001B_444A_497C_45A4 OR 4C530001040419118072
    /// </summary>
    public string SerialNo { get; set; } = null!;
    
    /// <summary>
    /// USB | SCSI | HDC | IDE | 1394 (Firewire) | Unknown
    /// </summary>
    public string? InterfaceType { get; set; }

    /// <summary>
    /// External hard disk media | Fixed hard disk media | Removable media | Unknown
    /// where Removable media is other than floppy
    /// </summary>
    public string? MediaType { get; set; }

    /// <summary>
    /// eg WD_BLACK SN750 SE 1TB OR SanDisk Ultra Fit USB Device
    /// </summary>
    public string Model { get; set; } = null!;
    
    /// <summary>
    /// eg WD_BLACK SN750 1
    /// </summary>
    public string DisplayName { get; set; } = null!;
    
    /// <summary>
    /// when this model was created 
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}