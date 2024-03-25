using System;
namespace COS20007Portfolio
{
  class Program
  {
    static void Main(string[] args)
    {
      Clock clock = new Clock();

      for (int i = 0; i < 10; i++) {
        Console.WriteLine($"Current Time: {clock.CurrentTime()}");
        clock.Tick();
        Thread.Sleep(1000);
      }
    }
  }
}
