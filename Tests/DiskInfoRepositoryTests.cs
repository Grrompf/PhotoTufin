using PhotoTufin.Model;
using PhotoTufin.Repository;
using Tests.DataProvider;

namespace Tests;

[TestFixture]
public class DiskInfoRepositoryTests
{
    private static DiskInfoRepository Repository => new();
    private static DiskInfo DiskInfo => new()
    {
        InterfaceType = "SCSI",
        MediaType = "Fixed hard disk media",
        Model = "WD_BLACK SN750 SE 1TB",
        SerialNo = "E823_8FA6_BF53_0231_001B_444A_487C_75B4"
    };

    [SetUp]
    public void Setup()
    {
        Repository.DropTable();
    }
    
    [TearDown]
    public void TearDown()
    {
        Repository.DropTable();
    }
    
    [Test]
    public void HasIdAfterSave()
    {
        var info = DiskInfo;
        Repository.Save(info);
        
        Assert.That(info.Id, Is.Not.Zero);
    }
    
    [Test]
    public void FindByIdIsNotNull()
    {
        var info = DiskInfo;
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
        var list = DiskInfoGenerator.generate(3);
        foreach (var disk in list)
        {
            Repository.Save(disk);
        }
        
        var actual = Repository.FindAll().Count;
        Assert.That(actual, Is.GreaterThan(1));
    }
    
    [Test]
    public void UniqueSerialNoDuplicateIsIgnored()
    {
        var info = DiskInfo;
        Repository.Save(info);
        Repository.Save(info); //ignored
        
        var actual = Repository.FindAll();
        Assert.That(actual, Has.Exactly(1).Items);
    }
    
    [Test]
    public void FindBySerialNumber()
    {
        var info = DiskInfo;
        Repository.Save(info);
        
        var actual = Repository.FindBySerialNo(info.SerialNo);
        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.Not.Null);
            Assert.That(info.Id, Is.EqualTo(actual?.Id));
        });
    }
    
    [Test]
    public void DeleteById()
    {
        var info = DiskInfo;
        Repository.Save(info);
        
        Repository.DeleteById(info.Id);
        var actual = Repository.FindAll();
        Assert.That(actual, Has.Exactly(0).Items);
    }
}