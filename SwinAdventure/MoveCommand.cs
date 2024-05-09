namespace SwinAdventure;

public class MoveCommand : Command
{
    public MoveCommand() : base(new string[] { "move", "go", "head", "leave" })
    {

    }
    
    public override string Execute(Player p, string[] text)
    {
        if (text.Length == 1  && text[0] != "leave" || text.Length > 3)
        {
            return "I don't know how to move like that";
        }

        if (text[0] != "move" && text[0] != "go" && text[0] != "head" && text[0] != "leave")
        {
            return "Error in move input";
        }

        if (text[0] != "leave")
        {
            if (text[1] != "to" || text.Length < 3)
            {
                return "where do you want to go?";
            }
            else
            {
                return MoveTo(p, text[2]);
            }
        }
        else
        {
            return MoveTo(p, "leave");
        }
    }

    private string MoveTo(Player p, string id)
    {
        Location loc;
        if (id == "leave")
        {
            p.LeaveLocation();
            return p.Location!.Name;
        }

        loc = p.Location!;

        Path path = loc.Path!;

        if (path == null)
            return "location has no path";

        loc = path.GetLocation(id);
        
        if (loc == null)
            return "path not found";

        p.Location = loc;
        return p.Location.FullDescription;
    }
}
