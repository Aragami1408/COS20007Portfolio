
namespace SimpleInterpreter;

public class SimpClass : ICallable
{
    public string name;
    private Dictionary<string, SimpFunction> methods;

    public SimpClass(string name, Dictionary<string, SimpFunction> methods)
    {
        this.name = name;
        this.methods = methods;
    }

    public SimpFunction findMethod(string name)
    {
	if (methods.ContainsKey(name))
	{
	    return methods[name];
	}

	return null;
    }

    public int Arity()
    {
	return 0;
    }

    public object call(Interpreter interpreter, List<object> arguments)
    {
	SimpInstance instance = new SimpInstance(this);
	return instance;
    }

    public override string? ToString()
    {
	return name;
    }
}
