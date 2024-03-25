using SwinAdventure;

namespace SwinAdventure.Tests;

public class IdentifiableObjectTest
{
    private IdentifiableObject id;

    [SetUp]
    public void Setup()
    {
        id = new IdentifiableObject(new string[] {"fred", "bob"});
    }

    [Test]
    public void TestAreYou()
    {
        Assert.IsTrue(id.AreYou("fred"));
        Assert.IsTrue(id.AreYou("bob"));
    }

    [Test]
    public void TestNotAreYou()
    {
        Assert.IsFalse(id.AreYou("wilma"));
        Assert.IsFalse(id.AreYou("boby"));
    }

    [Test]
    public void TestCaseInsensitive()
    {
        Assert.IsTrue(id.AreYou("FRED"));
        Assert.IsTrue(id.AreYou("bOB"));
    }

    [Test]
    public void TestFirstID()
    {
        Assert.That(id.FirstID(), Is.EqualTo("fred"));
    }

    [Test]
    public void TestFirstIDWithNoID() 
    {
        IdentifiableObject emptyID = new IdentifiableObject(new string[] {});

        Assert.That(emptyID.FirstID(), Is.EqualTo(""));        
    }

    [Test]
    public void TestAddID() {
        id.AddIdentifier("wilma");

        Assert.IsTrue(id.AreYou("Wilma"));
    }
}
