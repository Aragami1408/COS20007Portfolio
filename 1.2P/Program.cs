using System;
using System.Collections;
namespace COS20007Portfolio
{
  public class Program
  {
    static void Main(string[] args) 
    {
      Message myMessage = new Message("Hello World...");

      myMessage.Print();

      List<Message> messages = new List<Message>
      {
        new Message("Sup Wilma"),
            new Message("Sup Fred"),
            new Message("Sup Enoch"),
            new Message("Sup Jayden"),
            new Message("Sup Stranger"),
      };

      Console.Write("Enter name: ");

      string name = Console.ReadLine();

      if (name.ToLower() == "wilma")
      {
        messages[0].Print();
      }
      else if (name.ToLower() == "fred")
      {
        messages[1].Print();
      }
      else if (name.ToLower() == "enoch")
      {
        messages[2].Print();
      }
      else if (name.ToLower() == "jayden")
      {
        messages[3].Print();
      }
      else
      {
        messages[4].Print();
      }

    }
  }
}