namespace SwinAdventure;

public class Player : GameObject, IHaveInventory
{
    private Inventory _inventory;

    public Player(string name, string desc) : base( new string[] {"me", "inventory"}, name, desc)
    {
        _inventory = new Inventory();
    }


    public GameObject? Locate(string id)
    {
        if (AreYou(id))
            return this;
        else 
        {
            Item? fetchedItem = Inventory.Fetch(id);
            return fetchedItem;
        }
    }

    public override string FullDescription
    {
        get { return String.Format("You are {0} {1}.\n{2}", Name, Description, Inventory.ItemList); }
    }

    public Inventory Inventory
    {
        get { return _inventory; }
    }

}
