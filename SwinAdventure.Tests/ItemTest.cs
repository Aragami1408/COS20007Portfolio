using SwinAdventure;

namespace SwinAdventure.Tests;

public class ItemTest
{
    [Test]
    public void TestItemIsIdentifiable()
    {
        var shovel = new Item(new String[] {"shovel", "spade"},
                "a shovel",
                "this is a might fine...");

        Assert.IsTrue(shovel.AreYou("shovel"));
        Assert.IsTrue(shovel.AreYou("spade"));
    }

    [Test]
    public void TestShortDescription()
    {
        var sword = new Item(new String[] {"sword", "bronze"},
                "a bronze sword",
                "a sword that made by copper ingot");

        Assert.That(sword.ShortDescription, Is.EqualTo("a bronze sword (sword)"));

    }

    [Test]
    public void TestFullDescription()
    {
        var shovel = new Item(new String[] {"shovel", "spade"},
                "a shovel",
                "this is a might fine...");

        Assert.That(shovel.FullDescription, Is.EqualTo("this is a might fine..."));
    }
}
