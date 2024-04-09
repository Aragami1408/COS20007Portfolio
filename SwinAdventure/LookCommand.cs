namespace SwinAdventure;

public class LookCommand : Command
{
    public LookCommand() : base(new string[] {"look"})
    {

    }

    public override string Execute(Player p, string[] text)
    {
        IHaveInventory? container = null;
        GameObject? item = null;

        if (text.Length != 3 && text.Length != 5) {
            return "I don't know how to look like that";
        }

        if (text[0] != "look") {
            return "Error in look input";
        }
        
        if (text[1] != "at") {
            return "What do you want to look at?";
        }

        if (text.Length == 3) {
            container = FetchContainer(p, "inventory");
            item = container!.Locate(text[2]);
            if (item == null) {
                return "I cannot find the " + text[2];
            }
        }

        if (text.Length == 5) {
            if (text[3] != "in") {
                return "What do you want to look in?";
            }
            else {
                container = FetchContainer(p, text[4]);
                if (container == null) {
                    return "I cannot find the " + text[4];
                }
                else {

                    item = container!.Locate(text[2]);
                    if (item == null) {
                        return "I cannot find the " + text[2] + " in the " + container!.Name;
                    }
                }
            }
        }

        return item!.Description;
    }

    private IHaveInventory? FetchContainer(Player p, string containerId)
    {
        if (p.AreYou(containerId)) {
            return p;
        }
        else {
            GameObject? gameObject = p.Locate(containerId);
            if (gameObject is IHaveInventory) {
                IHaveInventory? haveInventory = gameObject as IHaveInventory;
                return haveInventory;
            }
        }

        return null;
    }

    private string LookAtIn(string thingId, IHaveInventory container)
    {
        throw new NotImplementedException();
    }
}
