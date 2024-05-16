
namespace SimpleInterpreter;

public class SimpFunction : ICallable
{
    private Stmt.Function declaration;

    public SimpFunction(Stmt.Function declaration)
    {
	this.declaration = declaration;
    }

    public int Arity()
    {
	return declaration.parameters.Count;
    }

    public object call(Interpreter interpreter, List<object> arguments)
    {
	Environment environment = new Environment(interpreter.globals);
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
