namespace SwinAdventure;

public class Path : IdentifiableObject
{
    private Dictionary<string, Location> _directions;

    public Path(string[] ids) : base(ids)
    {
        _directions = new Dictionary<string, Location>();
    }

    public void SetLocation(string direction, Location location)
    {
        if (location != null)
            _directions[direction] = location;
    }

    public Location? GetLocation(string direction)
    {
        if (_directions.ContainsKey(direction))
            return _directions[direction];

        foreach (var kvp in _directions)
        {
            if (kvp.Value.AreYou(direction))
                return kvp.Value;
        }

        return null;
    }
}
