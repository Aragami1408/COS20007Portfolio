namespace SwinAdventure;

public class IdentifiableObject
{
    private List<string> _identifiers;

    public IdentifiableObject(string[] idents) {
        _identifiers = new List<string>(idents);
    }

    public bool AreYou(string id) {
        return _identifiers.Any(str => str.Equals(id, StringComparison.OrdinalIgnoreCase));
    }

    public string FirstID() {
        if (_identifiers?.Any() != true) {
            return "";
        }
        else {
            return _identifiers[0];
        }
    }

    public void AddIdentifier(string id) {
        _identifiers.Add(id);        
    }
}
