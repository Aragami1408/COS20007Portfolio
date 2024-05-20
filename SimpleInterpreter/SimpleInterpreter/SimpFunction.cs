
namespace SimpleInterpreter;

public class SimpFunction : ICallable
{
    private Stmt.Function declaration;
    private Environment closure;

    public SimpFunction(Stmt.Function declaration, Environment closure)
    {
	this.closure = closure;
	this.declaration = declaration;
    }

    public SimpFunction bind(SimpInstance instance)
    {
	Environment environment = new Environment(closure);
	environment.define("this", instance);
	return new SimpFunction(declaration, environment);
    }

    public int Arity()
    {
	return declaration.parameters.Count;
    }

    public object call(Interpreter interpreter, List<object> arguments)
    {
	Environment environment = new Environment(closure);

	for (int i = 0; i < declaration.parameters.Count; i++)
	{
	    environment.define(declaration.parameters[i].lexeme, arguments[i]);
	}

	try
	{
	    interpreter.executeBlock(declaration.body, environment);
	}
	catch (SimpReturn returnValue)
	{
	    return returnValue.value;
	}
	return null;
    }

    public override string? ToString()
    {
	return "<fn " + declaration.name.lexeme + ">";
    }
}
