namespace SwinAdventure.Tests;

public class LocationTest
{
    Location? testLoc;

    [SetUp]
    public void LocationTestSetup()
    {
        testLoc = new Location(new string[] {"ruby", "gem"}, "ruby gem", "a shiny ruby gem");
    }

    [Test]
    public void TestAreYou()
    {
        Assert.IsTrue(testLoc!.AreYou("ruby"));
        Assert.IsTrue(testLoc!.AreYou("gem"));
    }

    [Test]
    public void TestLocate()
    {
        Item testItem = new Item(new string[] {"diamond", "pickaxe"}, "Diamond Pickaxe", "a pickaxe that is made from 2 sticks and 3 diamonds"); 
        testLoc!.Inventory.Put(testItem); 
        Assert.That(testLoc.Locate("diamond"), Is.EqualTo(testItem)); 
    }
    [Test] 
    public void TestPlayerLocate() { 
        Player testPlayer = new Player("Carl Johnson", "Our protagonist"); 
        Item testItem = new Item(new string[] {"diamond", "pickaxe"}, "Diamond Pickaxe", "a pickaxe that is made from 2 sticks and 3 diamonds"); 
        testPlayer.Location = testLoc!; 
        testLoc!.Inventory.Put(testItem); 
        Assert.That(testPlayer.Locate("diamond"), Is.EqualTo(testItem)); 
    } 
}
