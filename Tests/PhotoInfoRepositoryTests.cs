using PhotoTufin.Model;
using PhotoTufin.Repository;
using Tests.DataProvider;

namespace Tests;

[TestFixture]
public class PhotoInfoRepositoryTests
{
    private static PhotoInfoRepository Repository => new();
    private List<DiskInfo> DiskInfoList { get; set; } = null!;

    [SetUp]
    public void Setup()
    {
        Repository.DropTable();
        var repo = new DiskInfoRepository();
        var diskInfoList = DiskInfoGenerator.generate(3);
        var list = new List<DiskInfo>();
        foreach (var diskInfo in diskInfoList)
        {
            repo.Save(diskInfo);
            list.Add(diskInfo);
        }
        DiskInfoList = list;
    }

    [TearDown]
    public void TearDown()
    {
        Repository.DropTable();
    }
    
    [Test]
    public void HasIdAfterSave()
    {
        var diskInfo = DiskInfoGenerator.pickRandom(DiskInfoList);
        var info = PhotoInfoGenerator.generate(diskInfo).First();
        Repository.Save(info);
        
        Assert.That(info.Id, Is.Not.Zero);
    }
    
    [Test]
    public void FindByIdIsNotNull()
    {
        var diskInfo = DiskInfoGenerator.pickRandom(DiskInfoList);
        var info = PhotoInfoGenerator.generate(diskInfo).First();
        Repository.Save(info);
        var actual = Repository.Find(info.Id);
        
        Assert.That(actual, Is.Not.Null);
    }
    
    [Test]
    public void FindByIdIsNull()
    {
        var actual = Repository.Find(1);
        
        Assert.That(actual, Is.Null);
    }
    
    [Test]
    public void FindAllIsMoreThanOne()
    {
        const int count = 5;
        var diskInfo = DiskInfoGenerator.pickRandom(DiskInfoList);
        var photoList = PhotoInfoGenerator.generate(diskInfo, count);
        foreach (var photoInfo in photoList)
        {
            Repository.Save(photoInfo);
        }
        
        var actual = Repository.FindAll().Count;
        Assert.That(actual, Is.EqualTo(count));
    }
    
    [Test]
    public void DeleteById()
    {
        var diskInfo = DiskInfoGenerator.pickRandom(DiskInfoList);
        var info = PhotoInfoGenerator.generate(diskInfo).First();
        Repository.Save(info);
        
        Repository.DeleteByDiskInfo(info.DiskInfoId);
        var actual = Repository.FindAll();
        Assert.That(actual, Has.Exactly(0).Items);
    }
    
    [Test]
    public void FindDuplicates()
    {
        const int count = 5;
        var photoList = GeneratePhotos();
        
        var diskInfo = DiskInfoGenerator.pickRandom(DiskInfoList);
        var randomPhoto = PhotoInfoGenerator.pickRandom(photoList);
        var duplicateList = PhotoInfoGenerator.generateDuplicates(diskInfo, randomPhoto.HashString, count);
        
        foreach (var photoInfo in duplicateList)
        {
            Repository.Save(photoInfo);
        }
        // we expect on more duplicate since we have copied the hash 
        var actual = Repository.FindDuplicates();
        Assert.That(actual, Has.Exactly(count+1).Items);
    }
    
    [Test]
    public void FindDuplicatesByDiskInfo()
    {
        var photoList = GeneratePhotos();
        var hashString = PhotoInfoGenerator.pickRandom(photoList).HashString;

        //we create for each diskinfo an amount of duplicates
        foreach (var photoInfo in DiskInfoList.Select(diskInfo => PhotoInfoGenerator.generateDuplicates(diskInfo, hashString, 5)).SelectMany(duplicateList => duplicateList))
        {
            Repository.Save(photoInfo);
        }

        var diskInfo = DiskInfoGenerator.pickRandom(DiskInfoList);
        var actual = Repository.FindDuplicatesByDiskInfo(diskInfo.Id);
        Assert.That(actual, Has.Count.GreaterThanOrEqualTo(5));
    }
    
    [Test]
    public void FindDuplicatesByHash()
    {
        var photoList = GeneratePhotos();
        var hashString = PhotoInfoGenerator.pickRandom(photoList).HashString;

        //we create foreach diskinfo 5 duplicates (all together 3 * 5 + 1 = 16 duplicates)
        foreach (var photoInfo in DiskInfoList.Select(diskInfo => PhotoInfoGenerator.generateDuplicates(diskInfo, hashString, 5)).SelectMany(duplicateList => duplicateList))
        {
            Repository.Save(photoInfo);
        }

        var actual = Repository.FindDuplicatesByHashString(hashString);
        Assert.That(actual, Has.Exactly(16).Items);
    }

    /// <summary>
    /// Generates 10 random photos
    /// </summary>
    /// <returns></returns>
    private List<PhotoInfo> GeneratePhotos()
    {
        var diskInfo = DiskInfoGenerator.pickRandom(DiskInfoList);
        var photoList = PhotoInfoGenerator.generate(diskInfo, 10);
        foreach (var photoInfo in photoList)
        {
            Repository.Save(photoInfo);
        }

        return photoList;
    }
}