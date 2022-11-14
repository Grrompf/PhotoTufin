using System.Collections.Generic;
using PhotoTufin.Model;

namespace PhotoTufin.Repository;

public interface IDiskInfoRepository
{
    /// <summary>
    /// Find DiskInfo by unique serial number. Returns null if not found. 
    /// </summary>
    /// <param name="id"></param>
    /// <returns>DiskInfo?</returns>
    DiskInfo? Find(long id);
    
    /// <summary>
    /// Find DiskInfo by unique serial number. Returns null if not found. 
    /// </summary>
    /// <param name="serialNo"></param>
    /// <returns>DiskInfo?</returns>
    DiskInfo? FindBySerialNo(string serialNo);

    /// <summary>
    /// Returns a list of all DiskInfo. 
    /// </summary>
    /// <returns>List</returns>
    public List<DiskInfo> FindAll();
    
    /// <summary>
    /// Saves DiskInfo if not alreaday existing. 
    /// </summary>
    /// <param name="diskInfo"></param>
    void Save(DiskInfo diskInfo);

    /// <summary>
    /// Delete DiskInfo (volume) by its Id.
    /// Make sure, you have cleared the dependant data by
    /// using i.e. "DeleteByDiskInfo(int diskInfoId)". 
    /// </summary>
    /// <param name="diskInfoId"></param>
    public void DeleteById(int diskInfoId);
}