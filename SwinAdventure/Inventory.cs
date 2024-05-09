using System.Text;
namespace SwinAdventure;

public class Inventory
{
    private List<Item> _items;

    public Inventory()
    {
        _items = new List<Item>();
    }

    public bool HasItem(string id)
    {
        foreach (var item in _items)
        {
            if (item.AreYou(id))
                return true;
        }

        return false;
    }

    public void Put(Item itm)
    {
        _items.Add(itm);
    }

    public Item? Take(string id)
    {
        foreach (var item in _items)
        {
            if (item.AreYou(id))
            {
                var result = item;
                _items.Remove(item);
                return result;
            }
        }

        return null;
    }

    public Item? Fetch(string id)
    {
        foreach (var item in _items)
        {
            if (item.AreYou(id))
                return item;
        }
        
        return null;
    }

    public string ItemList
    {
        get { 
            StringBuilder sb = new StringBuilder(""); 
            sb.Append("\n");
            foreach (var item in _items)
            {
                sb.Append("    "); // Or use "\t" if your editor uses tabs
                sb.AppendFormat(item.ShortDescription);
                sb.Append("\n");
            }

            return sb.ToString();
        }
    }
}
