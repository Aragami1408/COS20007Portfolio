namespace SwinAdventure.Tests;

public class BagTest
{
    private Bag bag;

    [SetUp]
    public void SetupBag()
    {
        bag = new Bag(new string[] {"bag", "testing"}, "Testing Bag", "This bag is used solely for testing");
    }

    [Test]
    public void TestBagLocatesItems()
    {
        bag.Inventory.Put(new Item(new string[] {"shovel", "spade"}, "a shovel", "This might be a fine ..."));
        bag.Inventory.Put(new Item(new string[] {"sword", "copper", "bronze"}, "a bronze sword", "This is a sword that made by copper ingots"));
        bag.Inventory.Put(new Item(new string[] {"pc", "machine"}, "a small computer", "a personal computer that can be used to run softwares"));

        GameObject? locatedGameObject;

        locatedGameObject = bag.Locate("shovel");
        Assert.That(locatedGameObject.Name, Is.EqualTo("a shovel"));
        locatedGameObject = bag.Locate("spade");
        Assert.That(locatedGameObject.Name, Is.EqualTo("a shovel"));

        locatedGameObject = bag.Locate("sword");
        Assert.That(locatedGameObject.Name, Is.EqualTo("a bronze sword"));
        locatedGameObject = bag.Locate("copper");
        Assert.That(locatedGameObject.Name, Is.EqualTo("a bronze sword"));
        locatedGameObject = bag.Locate("bronze");
        Assert.That(locatedGameObject.Name, Is.EqualTo("a bronze sword"));

        locatedGameObject = bag.Locate("pc");
        Assert.That(locatedGameObject.Name, Is.EqualTo("a small computer"));
        locatedGameObject = bag.Locate("machine");
        Assert.That(locatedGameObject.Name, Is.EqualTo("a small computer"));
    }

    [Test]
    public void TestBagLocatesItself()
    {
        GameObject? locatedGameObject = bag.Locate("bag");
        Assert.That(locatedGameObject.Name, Is.EqualTo("Testing Bag"));
        locatedGameObject = bag.Locate("testing");
        Assert.That(locatedGameObject.Name, Is.EqualTo("Testing Bag"));
    }

    [Test]
    public void TestBagLocatesNothing()
    {
        GameObject? locatedGameObject = bag.Locate("pickaxe");
        Assert.IsNull(locatedGameObject);
    }

    [Test]
    public void TestBagFullDescription()
    {
        bag.Inventory.Put(new Item(new string[] {"shovel", "spade"}, "a shovel", "This might be a fine ..."));
        bag.Inventory.Put(new Item(new string[] {"sword", "copper", "bronze"}, "a bronze sword", "This is a sword that made by copper ingots"));
        bag.Inventory.Put(new Item(new string[] {"pc", "machine"}, "a small computer", "a personal computer that can be used to run softwares"));

        string actualString = @"In the Testing Bag you can see:
You are carrying
    a shovel (shovel)
    a bronze sword (sword)
    a small computer (pc)
";

        actualString = actualString.Replace("\r\n", "\n");
        Assert.That(bag.FullDescription, Is.EqualTo(actualString));
    }

    [Test]
    public void TestBagInBag()
    {
        Bag b1 = new Bag(new string[] {"bag1", "firstbag"}, "Bag one", "This is the first bag for testing");
        Bag b2 = new Bag(new string[] {"bag2", "secondbag"}, "Bag two", "This is the second bag for testing");

        b1.Inventory.Put(new Item(new string[] {"shovel", "spade"}, "a shovel", "This might be a fine ..."));
        b1.Inventory.Put(b2);

        b2.Inventory.Put(new Item(new string[] {"sword", "copper", "bronze"}, "a bronze sword", "This is a sword that made by copper ingots"));

        GameObject? locatedGameObject = b1.Locate("bag2");
        Assert.That(locatedGameObject.Name, Is.EqualTo("Bag two"));

        locatedGameObject = b1.Locate("shovel");
        Assert.IsTrue(locatedGameObject is Item);

        locatedGameObject = b1.Locate("sword");
        Assert.IsTrue(locatedGameObject == null);
    }
}
