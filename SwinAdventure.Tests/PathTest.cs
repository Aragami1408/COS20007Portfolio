namespace SwinAdventure.Tests;

public class PathTest
{
    Player? player;
    Location? loc1, loc2, loc3;
    Path? path1, path2, path3;

    [SetUp]
    public void PathSetup()
    {
        player = new Player("player", "test player");

        loc1 = new Location(new string[] {"hakurei", "jinja"}, "hakurei jinja", "Shrine of Hakurei");
        loc2 = new Location(new string[] {"moriya", "jinja"}, "moriya jinja", "Shrine of Moriya");
        loc3 = new Location(new string[] {"netherworld"}, "netherworld", "Netherworld");

        path1 = new Path(new string[] { "hakurei", "path" });
        path2 = new Path(new string[] { "moriya", "path" });
        path3 = new Path(new string[] { "netherworld", "path" });

        loc1.Path = path1;  
        path1.SetLocation("west", loc2);
        path1.SetLocation("north", loc3);

        loc2.Path = path2;
        path2.SetLocation("east", loc1);
        path2.SetLocation("north_east", loc3);

        loc3.Path = path3;
        path3.SetLocation("south", loc1);
        path3.SetLocation("south_west", loc3);

        player.Location = loc1;
    }

    [Test]
    public void TestPathMove()
    {
        MoveCommand move = new MoveCommand();
        Assert.That(move.Execute(player, new string[] {"move", "to", "west"}), Is.EqualTo(loc2.Name));
        Assert.That(move.Execute(player, new string[] {"move", "to", "north_east"}), Is.EqualTo(loc3.Name));
    }

    [Test]
    public void TestGetPath()
    {
        Assert.IsTrue(loc1!.Path == path1);
    }

    [Test]
    public void TestLeave()
    {
        MoveCommand move = new MoveCommand();
        Assert.That(move.Execute(player, new string[] {"move", "to", "west"}), Is.EqualTo(loc2.Name));
        Assert.That(move.Execute(player, new string[] {"leave"}), Is.EqualTo(loc1.Name));
    }

    [Test]
    public void TestLeaveFail()
    {
        MoveCommand move = new MoveCommand();
        Assert.That(move.Execute(player, new string[] {"move", "to", "heaven"}), Is.EqualTo("path not found"));
        Assert.That(move.Execute(player, new string[] {"lmao"}), Is.EqualTo("I don't know how to move like that"));
    }
}
