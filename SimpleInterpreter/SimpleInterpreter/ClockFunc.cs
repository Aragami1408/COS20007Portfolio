
namespace SimpleInterpreter;

public class ClockFunc : ICallable
{
    public int Arity()
    {
	  return 0;
    }

    public object call(Interpreter interpreter, List<object> arguments)
    {
	  TimeSpan t = (DateTime.UtcNow - new DateTime(1970,1,1));
	  return (double)t.TotalSeconds;
    }


    public override string? ToString()
    {
	  return "<native fn>";
    }
}
