
namespace SimpleInterpreter;

public class ClockFunc : ICallable
{
    public int Arity()
    {
	return 0;
    }

    public object call(Interpreter interpreter, List<object> arguments)
    {
	return DateTime.Now.Second + DateTime.Now.Millisecond / 1000.0;
    }


    public override string? ToString()
    {
	return "<native fn>";
    }
}
