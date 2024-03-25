namespace COS20007Portfolio;

public class Program
{
    static void Main(string[] args) 
    {
        IntegerQueue queue = new IntegerQueue();

        queue.Enqueue(10);
        queue.Enqueue(20);
        queue.Enqueue(30);

        Console.WriteLine(queue.Count);
        Console.WriteLine(queue.Dequeue());
        Console.WriteLine(queue.Dequeue());
        Console.WriteLine(queue.Dequeue());
    }
}
