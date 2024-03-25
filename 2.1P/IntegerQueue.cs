namespace COS20007Portfolio;

public class IntegerQueue
{
    public List<int> _elements;

    public IntegerQueue()
    {
        _elements = new List<int>();    
    }

    public int Count
    {
        get 
        {
            return _elements.Count;
        }
    }

    public void Enqueue(int value)
    {
        _elements.Add(value);
    }

    public int Dequeue()
    {
        int result = _elements[0];
        _elements.RemoveAt(0);
        return result;
    }
        
}
