using System.Collections.Generic;
using PhotoTufin.Model;

namespace PhotoTufin.Repository;

public interface IPhotoInfoRepository
{
    /// <summary>
    /// Find PhotoInfo by unique serial number. Returns null if not found. 
    /// </summary>
    /// <param name="id"></param>
    /// <returns>PhotoInfo?</returns>
    PhotoInfo? Find(long id);
    
    /// <summary>
    /// Returns a list of all duplicates in database.
    /// </summary>
    /// <returns>List</returns>
    List<PhotoInfo> FindDuplicates();

    /// <summary>
    /// Find alle duplicates of a volume 
    /// </summary>
    /// <param name="diskInfoId"></param>
    /// <returns>List</returns>
    public List<PhotoInfo> FindDuplicatesByDiskInfo(long diskInfoId);

    /// <summary>
    /// Find alle duplicates by its hash and inner request for having a duplicate.
    /// </summary>
    /// <param name="hashString"></param>
    /// <returns>List</returns>
    public List<PhotoInfo> FindDuplicatesByHashString(string hashString);

    /// <summary>
    /// Returns a list of all PhotoInfo. 
    /// </summary>
    /// <returns>List</returns>
    public List<PhotoInfo> FindAll();
    
    /// <summary>
    /// Saves PhotoInfo if not alreaday existing. 
    /// </summary>
    /// <param name="photoInfo"></param>
    void Save(PhotoInfo photoInfo);
    
    /// <summary>
    /// Deletes all PhotoInfo of a DiskInfo (volume).   
    /// </summary>
    /// <param name="diskInfoId"></param>
    void DeleteByDiskInfo(long diskInfoId);

    /// <summary>
    /// Get the number of all images on a disk (Volume)
    /// </summary>
    /// <param name="diskInfoId"></param>
    /// <returns></returns>
    int GetImageCount(long diskInfoId);
}