namespace SwinAdventure;

public abstract class GameObject : IdentifiableObject
{
    private string _name;
    private string _description;

    public GameObject(string[] ids, string name, string description) : base(ids)
    {
        _name = name;
        _description = description;
    }

    public String Name
    {
        get {return _name;}
    }
    
    public String Description
    {
        get {return _description;}
    }

    public String ShortDescription
    {
        get {return String.Format("{0} ({1})", _name, FirstID());}
    }

    public virtual string FullDescription
    {
        get {return Description;}
    }
}
