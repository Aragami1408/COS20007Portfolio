using System.Text;
namespace SwinAdventure;

public class Bag : Item
{
    private Inventory _inventory;

    public Bag(string[] ids, string name, string desc) 
        : base(ids, name, desc)
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
        get 
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("In the {0} you can see:\n", Name);
            sb.Append(Inventory.ItemList);

            return sb.ToString();
        }
    }

    public Inventory Inventory
    {
        get { return _inventory; }
    }
}
