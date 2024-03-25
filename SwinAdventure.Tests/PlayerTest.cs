namespace SwinAdventure.Tests;

public class PlayerTest
{
    private Player player;

    [SetUp]
    public void SetupPlayer()
    {
        player = new Player("Fred", "the mighty programmer");
        player.Inventory.Put(new Item(new string[] {"shovel", "spade"}, "a shovel", "This might be a fine ..."));
        player.Inventory.Put(new Item(new string[] {"sword", "copper", "bronze"}, "a bronze sword", "This is a sword that made by copper ingots"));
        player.Inventory.Put(new Item(new string[] {"pc", "machine"}, "a small computer", "a personal computer that can be used to run softwares"));
    }

    [Test]
    public void TestPlayerIsIdentifiable()
    {
        Assert.IsTrue(player.AreYou("me"));
        Assert.IsTrue(player.AreYou("inventory"));
    }

    [Test]
    public void TestPlayerLocatesItems()
    {
        GameObject? locatedGameObject;

        locatedGameObject = player.Locate("shovel");
        Assert.IsTrue(locatedGameObject is Item);
        locatedGameObject = player.Locate("spade");
        Assert.IsTrue(locatedGameObject is Item);

        locatedGameObject = player.Locate("sword");
        Assert.IsTrue(locatedGameObject is Item);
        locatedGameObject = player.Locate("copper");
        Assert.IsTrue(locatedGameObject is Item);
        locatedGameObject = player.Locate("bronze");
        Assert.IsTrue(locatedGameObject is Item);

        locatedGameObject = player.Locate("pc");
        Assert.IsTrue(locatedGameObject is Item);
        locatedGameObject = player.Locate("machine");
        Assert.IsTrue(locatedGameObject is Item);
    }

    [Test]
    public void TestPlayerLocatesItself()
    {
        GameObject? locatedGameObject = player.Locate("me");
        Assert.IsTrue(locatedGameObject is Player);
        locatedGameObject = player.Locate("inventory");
        Assert.IsTrue(locatedGameObject is Player);
    }

    [Test]
    public void TestPlayerLocatesNothing()
    {
        GameObject? locatedGameObject = player.Locate("pickaxe");
        Assert.IsNull(locatedGameObject);
    }

    [Test]
    public void TestPlayerFullDescription()
    {
        string actualString = @"You are Fred the mighty programmer.
You are carrying
    a shovel (shovel)
    a bronze sword (sword)
    a small computer (pc)
";

        actualString = actualString.Replace("\r\n", "\n");
        Assert.That(player.FullDescription, Is.EqualTo(actualString));
    }
}
