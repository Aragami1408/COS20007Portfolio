namespace SwinAdventure;

public class Player : GameObject, IHaveInventory
{
    private Inventory _inventory;
    private Location? _location;
    private Location? _lastLocation;

    public Player(string name, string desc) : base( new string[] {"me", "inventory"}, name, desc)
    {
        _inventory = new Inventory();
        _location = null;
        _lastLocation = null;
    }


    public GameObject? Locate(string id)
    {
        if (AreYou(id))
            return this;
        else if (Location != null)
            return Location.Locate(id);
        else if  (Inventory.HasItem(id))
            return Inventory.Fetch(id);
        else 
        {
            return null;
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

    public Location? Location { 
        get => _location; 
        set 
        {
            _lastLocation = (_location == null) ? value : _location;
            _location = value;
        }
    }

    public void LeaveLocation() 
    {
        _location = _lastLocation;
    }
}
