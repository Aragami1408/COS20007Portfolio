namespace SwinAdventure;

public class Location : GameObject, IHaveInventory
{
    private Inventory _inventory;
    private Path? _path;

    public Location(string[] ids, string name, string desc) : base(ids, name, desc)
    {
        _inventory = new Inventory();
    }

    public GameObject? Locate(string id)
    {
        if (AreYou(id))
            return this;
        else if (Inventory.HasItem(id))
        {
            return Inventory.Fetch(id);
        }
        return null;
    }

    public Inventory Inventory
    {
        get { return _inventory; }
        set { _inventory = value; }
    }

    public Path? Path { get => _path; set => _path = value; }
}
