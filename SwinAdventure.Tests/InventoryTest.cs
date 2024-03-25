using SwinAdventure;

namespace SwinAdventure.Tests;

public class InventoryTest
{

    private Inventory inventory;

    [SetUp]
    public void InventorySetup()
    {
        Item shovel = new Item(new String[] {"shovel", "spade"},
                "a shovel",
                "this is a might fine...");

        Item sword = new Item(new String[] {"sword", "bronze"},
                "a bronze sword",
                "a sword that made by copper ingot");
         
        inventory = new Inventory();

        inventory.Put(shovel);
        inventory.Put(sword);
    }

    [Test]
    public void TestFindItem()
    {
        Assert.IsTrue(inventory.HasItem("shovel"));
        Assert.IsTrue(inventory.HasItem("spade"));
        Assert.IsTrue(inventory.HasItem("sword"));
        Assert.IsTrue(inventory.HasItem("bronze"));
    }

    [Test]
    public void TestNoItemFind()
    {
        Assert.IsFalse(inventory.HasItem("pickaxe"));
    }

    [Test]
    public void TestFetchItem()
    {
        Item? fetchedItem = inventory.Fetch("bronze");
        Assert.That(fetchedItem?.Name, Is.EqualTo("a bronze sword")); 
    }

    [Test]
    public void TestTakeItem()
    {
        Item? fetchedItem = inventory.Fetch("shovel");
        Assert.That(fetchedItem?.Name, Is.EqualTo("a shovel")); 

        Item? takenItem = inventory.Take("spade");
        Assert.IsFalse(inventory.HasItem("spade"));
        Assert.IsFalse(inventory.HasItem("shovel"));
        
    }

    [Test]
    public void TestItemList()
    {
        string actualString = @"You are carrying
    a shovel (shovel)
    a bronze sword (sword)
";

        actualString = actualString.Replace("\r\n", "\n");

        Assert.That(inventory.ItemList, Is.EqualTo(actualString));
    }
}
