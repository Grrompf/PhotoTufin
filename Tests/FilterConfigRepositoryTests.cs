using PhotoTufin.Model;
using PhotoTufin.Repository;
using Tests.DataProvider;

namespace Tests;

[TestFixture]
public class FilterConfigRepositoryTests
{
    private static FilterConfigRepository Repository => new();
    private static FilterConfig FilterConfig => new()
    {
        Filter = "jpg, jpeg, jpe, jfif",
        IsChecked = true
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
        var info = FilterConfig;
        Repository.Save(info);
        
        Assert.That(info.Id, Is.Not.Zero);
    }
    
    [Test]
    public void UniqueFilterIsIgnored()
    {
        var info = FilterConfig;
        Repository.Save(info);
        Repository.Save(info); //ignored
        
        var actual = Repository.FindAll();
        Assert.That(actual, Has.Exactly(1).Items);
    }
    
    [Test]
    public void FindByFilter()
    {
        var info = FilterConfig;
        Repository.Save(info);
        
        var actual = Repository.FindByFilter(info.Filter);
        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.Not.Null);
            Assert.That(info.Id, Is.EqualTo(actual?.Id));
        });
    }
    
    [Test]
    public void FindAllIsMoreThanOne()
    {
        var info = FilterConfig;
        Repository.Save(info);
        
        const int count = 5;
        var filterList = FilterGenerator.generate(count);
        foreach (var filter in filterList)
        {
            Repository.Save(filter);
        }
        
        var actual = Repository.FindAll().Count;
        Assert.That(actual, Is.GreaterThanOrEqualTo(2));
    }
}