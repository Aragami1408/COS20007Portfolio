namespace SwinAdventure.Tests;

public class LookCommandTest
{
    private Player player;
    private LookCommand lookCommand;

    [SetUp]
    public void SetupLookupScene()
    {
        player = new Player("Fred", "the mighty programmer");
        lookCommand = new LookCommand();

    }

    [Test]
    public void TestLookAtMe()
    {
        string item = lookCommand.Execute(player, new string[] {"look", "at", "inventory"});

        Assert.That(item, Is.EqualTo("Fred"));
    }

    [Test]
    public void TestLookAtGem()
    {
        player.Inventory.Put(new Item(new string[] {"gem"}, "a gem", "this is a shiny gem"));
        string item = lookCommand.Execute(player, new string[] {"look", "at", "gem"});

        Assert.That(item, Is.EqualTo("a gem"));
    }

    [Test]
    public void TestLookAtUnk()
    {
        string item = lookCommand.Execute(player, new string[] {"look", "at", "gem"});
        
        Assert.That(item, Is.EqualTo("I cannot find the gem"));
    }

    [Test]
    public void TestLookAtGemInMe()
    {
        player.Inventory.Put(new Item(new string[] {"gem"}, "a gem", "this is a shiny gem"));
        string item = lookCommand.Execute(player, new string[] {"look", "at", "gem", "in", "inventory"});

        Assert.That(item, Is.EqualTo("a gem"));
        
    }

    [Test]
    public void TestLookAtGemInBag()
    {
        Bag bag = new Bag(new string[] {"bag1"}, "Bag 1", "First Bag");
        bag.Inventory.Put(new Item(new string[] {"gem"}, "a gem", "this is a shiny gem"));
        player.Inventory.Put(bag);
        string item = lookCommand.Execute(player, new string[] {"look", "at", "gem", "in", "bag1"});

        Assert.That(item, Is.EqualTo("a gem"));
    }

    [Test]
    public void TestLookAtGemInNoBag()
    {
        player.Inventory.Put(new Item(new string[] {"gem"}, "a gem", "this is a shiny gem"));
        string item = lookCommand.Execute(player, new string[] {"look", "at", "gem", "in", "no_bag"});

        Assert.That(item, Is.EqualTo("I cannot find the no_bag"));
        
    }

    [Test]
    public void TestLookAtNoGemInBag()
    {
        Bag bag = new Bag(new string[] {"bag1"}, "Bag 1", "First Bag");
        player.Inventory.Put(bag);
        string item = lookCommand.Execute(player, new string[] {"look", "at", "gem", "in", "bag1"});

        Assert.That(item, Is.EqualTo("I cannot find the gem in bag1"));
        
    }

    [Test]
    public void TestInvalidLook()
    {
        string result1 = new LookCommand().Execute(player, new string[] {"look", "around"});
        string result2 = new LookCommand().Execute(player, new string[] {"look", "on", "me"});
        string result3 = new LookCommand().Execute(player, new string[] {"look", "at", "a", "at", "b"});
        string result4 = new LookCommand().Execute(player, new string[] {"hello", "it's", "me"});

        Assert.That(result1, Is.EqualTo("I don't know how to look like that"));
        Assert.That(result2, Is.EqualTo("What do you want to look at?"));
        Assert.That(result3, Is.EqualTo("What do you want to look in?"));
        Assert.That(result4, Is.EqualTo("Error in look input"));
    }
}
