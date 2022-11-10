using PhotoTufin.Model;

namespace PhotoTufin.Data;

public interface IDiskInfoRepository
{
    DiskInfo? Find(long id);
    void Save(DiskInfo diskInfo);
}