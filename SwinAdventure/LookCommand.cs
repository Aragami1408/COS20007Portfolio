namespace SwinAdventure;

public class LookCommand : Command
{
    public LookCommand() : base(new string[] {"look"})
    {

    }

    public override string Execute(Player p, string[] text)
    {

        if (text.Length != 3 && text.Length != 5) {
            return "I don't know how to look like that";
        }

        if (text[0] != "look") {
            return "Error in look input";
        }
        
        if (text[1] != "at") {
            return "What do you want to look at?";
        }

        if (text.Length == 5 && text[3] != "in")
        {
            return "What do you want to look in?";
        }
        else if (text.Length == 5)
        {
            return LookAtIn(p, text[2], text[4]);
        }
        else
        {
            return LookAtIn(p, text[2], "");
        }

    }

    private string LookAtIn(Player p, string thingId, string containerId)
    {
        if (p.AreYou(thingId))
            return p.Name;

        GameObject? item = null;

        IHaveInventory? inventory = FetchContainer(p, containerId);
        if (inventory == null)
            return "I cannot find the " + containerId;
        
        item = inventory.Locate(thingId);
        if (item == null)
        {
            if (containerId == null || containerId == "")
            {
                return "I cannot find the " + thingId;
            }
            else
            {
                return "I cannot find the " + thingId + " in " + containerId; 
            }
        }

        return item.Name;
    }

    private IHaveInventory? FetchContainer(Player p, string containerId)
    {
        
        IHaveInventory? inventory = p.Locate(containerId) as IHaveInventory;

        if (p.AreYou(containerId) || containerId == "")
        {
            inventory = (IHaveInventory) p;
        }

        return inventory;

    }

}
