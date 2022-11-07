namespace PhotoTufin.Search.SystemIO;

public class HDDInfo
{
    /// <summary>
    /// USB | SCSI | HDC | IDE | 1394 (Firewire)
    /// </summary>
    public string? InterfaceType { get; init; }
    
    /// <summary>
    /// External hard disk media | Fixed hard disk media | Removable media | Unknown
    /// where Removable media is other than floppy
    /// </summary>
    public string? MediaType { get; init; }
    
    /// <summary>
    /// eg WD_BLACK SN750 SE 1TB OR SanDisk Ultra Fit USB Device
    /// </summary>
    public string? Model { get; init; }
    
    /// <summary>
    /// eg E823_8FA6_BF53_0001_001B_444A_497C_45A4 OR 4C530001040419118072
    /// </summary>
    public string? SerialNo { get; init; }
}