using PhotoTufin.Model;
using PhotoTufin.Repository;

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
        SerialNo = "E823_8FA6_BF53_0001_001B_444A_497C_45A4"
    };

    [SetUp]
    public void Setup()
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
        var info = DiskInfo;
        Repository.Save(info);
        info.SerialNo = "test";
        Repository.Save(info);
        
        var actual = Repository.FindAll().Count;
        Assert.That(actual, Is.GreaterThan(1));
    }
    
    [Test]
    public void SerialNoIsUnique()
    {
        var info = DiskInfo;
        Repository.Save(info);
        Repository.Save(info);
        
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