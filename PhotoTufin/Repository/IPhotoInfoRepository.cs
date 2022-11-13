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
    /// Find PhotoInfo by unique serial number. Returns null if not found. 
    /// </summary>
    /// <returns>List</returns>
    List<PhotoInfo> FindDuplicates();

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
}