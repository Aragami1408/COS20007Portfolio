namespace SimpleInterpreter;

public class SimpInstance
{

    private SimpClass simpClass;
    private Dictionary<string, object> fields = new Dictionary<string, object>();

    public SimpInstance(SimpClass simpClass)
    {
        this.simpClass = simpClass;
    }

    public object get(Token name)
    {
	if (fields.ContainsKey(name.lexeme))
	    return fields[name.lexeme];

	SimpFunction method = simpClass.findMethod(name.lexeme);
	if (method != null) return method.bind(this);

	throw new RuntimeError(name, "Undefined property '" + name.lexeme + "'");
    }

    public void set(Token name, object value)
    {
	fields.Add(name.lexeme, value);
    }

    public override string? ToString()
    {
	return simpClass.name + " instance";
    }
}
