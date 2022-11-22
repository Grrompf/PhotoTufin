namespace PhotoTufin.Search.SystemIO;

/// <summary>
/// Model for disk infos. This is not an entity. It is a transfer data model of the scan. 
/// </summary>
public class HDDInfo
{
    private const string UNKNOWN = "Unknown";

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="model"></param>
    /// <param name="serialNo"></param>
    /// <param name="interfaceType"></param>
    /// <param name="mediaType"></param>
    public HDDInfo(string model, string serialNo, string? interfaceType, string? mediaType)
    {
        Model = model;
        SerialNo = serialNo;
        InterfaceType = interfaceType ?? UNKNOWN;
        MediaType = mediaType ?? UNKNOWN;
    }
    
    /// <summary>
    /// USB | SCSI | HDC | IDE | 1394 (Firewire)
    /// </summary>
    public string InterfaceType { get; }
    
    /// <summary>
    /// External hard disk media | Fixed hard disk media | Removable media | Unknown
    /// where Removable media is other than floppy
    /// </summary>
    public string MediaType { get; }

    /// <summary>
    /// eg WD_BLACK SN750 SE 1TB OR SanDisk Ultra Fit USB Device
    /// </summary>
    public string Model { get; } 

    /// <summary>
    /// eg E823_8FA6_BF53_0001_001B_444A_497C_45A4 OR 4C530001040419118072
    /// </summary>
    public string SerialNo { get; }
}