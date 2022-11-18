using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Windows.Controls;
using PhotoTufin.Model;
using PhotoTufin.Repository;
using PhotoTufin.Search.Duplication;
using PhotoTufin.Search.Filter;
using PhotoTufin.Search.SystemIO;

namespace PhotoTufin.Search;

[SupportedOSPlatform("windows")]
public class ImageInfoFactory
{
    public ImageInfoFactory(string selectedPath, MenuItem filter)
    {
        SelectedPath = selectedPath;
        Filter = filter;
        DiskReader = new HDDInfoReader(selectedPath);
    }
    
    public int NoDuplicates { get; private set; }
    
    public HDDInfo? HDDInfo { get; set;  }

    private string SelectedPath { get; }

    private HDDInfoReader DiskReader { get; }
    
    private MenuItem Filter { get; }

    private DiskInfoRepository DiskInfoRepository { get; } = new();
    
    private PhotoInfoRepository PhotoInfoRepository { get; } = new();
    
    public List<ImageInfo> findImages()
    {
        // get wmi infos of the hdd
        HDDInfo = DiskReader.GetDiskInfo();
        var diskInfo = CreateDiskInfo(HDDInfo);
        
        var fileExtensions = ImageFilter.makeFilter(Filter);
        var search = new WalkFolders(SelectedPath, fileExtensions);
        var imageInfos = search.search();

        NoDuplicates = DuplicateFinder.findDuplicates(ref imageInfos);

        foreach (var image in imageInfos)
        {
            CreatePhotoInfo(diskInfo, image);
        }

        return imageInfos;
    }

    private DiskInfo? CreateDiskInfo(HDDInfo? hddInfo)
    {
        if (hddInfo?.SerialNo == null) return null;
        
        var diskInfo = DiskInfoRepository.FindBySerialNo(hddInfo.SerialNo);
        if (diskInfo != null) return diskInfo;
        
        diskInfo = new DiskInfo
        {
            Model = hddInfo.Model,
            InterfaceType = hddInfo.InterfaceType,
            MediaType = hddInfo.MediaType,
            SerialNo = hddInfo.SerialNo
        };
        DiskInfoRepository.Save(diskInfo);
        
        return diskInfo;
    }
    
    private void CreatePhotoInfo(IModel? diskInfo, ImageInfo imageInfo)
    {
        if (diskInfo == null || imageInfo.HashString == null) return;
        var photo = new PhotoInfo
        {
            DiskInfoId = diskInfo.Id,
            HashString = imageInfo.HashString,
            FileName = imageInfo.FileName,
            FilePath = imageInfo.FilePath,
            Size = imageInfo.Size
        };
        PhotoInfoRepository.Save(photo);
    }
}