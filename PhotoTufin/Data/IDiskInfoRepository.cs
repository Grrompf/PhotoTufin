using PhotoTufin.Model;

namespace PhotoTufin.Data;

public interface IDiskInfoRepository
{
    DiskInfo GetDiskInfo(long id);
    void SaveDiskInfo(DiskInfo diskInfo);
}