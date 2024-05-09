namespace SwinAdventure.Tests;

public class CommandProcessorTest
{
    [Test]
    public void TestLookCommand()
    {
        Player player = new Player("Steve", "Minecraft");
        Command look = new LookCommand();
        CommandProcessor proc = new CommandProcessor();
        proc.AddCommand(look);
        Assert.That(proc.Execute(player, new string[3] {"look", "at", "me"}), Is.EqualTo(player.FullDescription));
    }

    [Test]
    public void TestMoveCommand()
    {
        Player player = new Player("player", "test player");

        Location loc1 = new Location(new string[] {"hakurei", "jinja"}, "hakurei jinja", "Shrine of Hakurei");
        Location loc2 = new Location(new string[] {"moriya", "jinja"}, "moriya jinja", "Shrine of Moriya");
        Location loc3 = new Location(new string[] {"netherworld"}, "netherworld", "Netherworld");

        Path path1 = new Path(new string[] { "hakurei", "path" });
        Path path2 = new Path(new string[] { "moriya", "path" });
        Path path3 = new Path(new string[] { "netherworld", "path" });

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

        Command move = new MoveCommand();
        CommandProcessor proc = new CommandProcessor();

        proc.AddCommand(move);

        Assert.That(proc.Execute(player, new string[] {"move", "to", "west"}), Is.EqualTo(loc2.FullDescription));
        Assert.That(proc.Execute(player, new string[] {"move", "to", "north_east"}), Is.EqualTo(loc3.FullDescription));
    }

    [Test]
    public void TestLookAndMoveCommand()
    {
        Player player = new Player("player", "test player");

        Location loc1 = new Location(new string[] {"hakurei", "jinja"}, "hakurei jinja", "Shrine of Hakurei");
        Location loc2 = new Location(new string[] {"moriya", "jinja"}, "moriya jinja", "Shrine of Moriya");
        Location loc3 = new Location(new string[] {"netherworld"}, "netherworld", "Netherworld");

        Path path1 = new Path(new string[] { "hakurei", "path" });
        Path path2 = new Path(new string[] { "moriya", "path" });
        Path path3 = new Path(new string[] { "netherworld", "path" });

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

        Command look = new LookCommand();
        Command move = new MoveCommand();
        CommandProcessor proc = new CommandProcessor();

        proc.AddCommand(look);
        proc.AddCommand(move);
 
        Assert.That(proc.Execute(player, new string[3] {"look", "at", "me"}), Is.EqualTo(player.FullDescription));
        Assert.That(proc.Execute(player, new string[] {"move", "to", "west"}), Is.EqualTo(loc2.FullDescription));
        Assert.That(proc.Execute(player, new string[] {"move", "to", "north_east"}), Is.EqualTo(loc3.FullDescription));
    }

}
