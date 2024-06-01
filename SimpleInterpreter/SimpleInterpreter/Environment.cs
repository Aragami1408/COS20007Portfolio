namespace SimpleInterpreter;

public class Environment
{
  private readonly Dictionary<string, object> values = new Dictionary<string, object>();
  public readonly Environment enclosing;

  public Environment()
  {
    
  }

  public Environment(Environment enclosing)
  {
    this.enclosing = enclosing;
  }

  public void define(string name, object value)
  {
    values.Add(name, value); 
  }

  public object get(Token name)
  {
    if (values.ContainsKey(name.lexeme))
    {
      return values[name.lexeme];
    }

    if (enclosing != null) 
      return enclosing.get(name);

    throw new RuntimeError(name, "Undefined variable '" + name.lexeme + "'.");
  }

  public void assign(Token name, object value)
  {
    if (values.ContainsKey(name.lexeme))
    {
      values[name.lexeme] = value;
      return;
    }

    if (enclosing != null) 
    {
      enclosing.assign(name, value);
      return;
    }

    throw new RuntimeError(name, "Undefined variable '"  + name.lexeme + "'.");
  }

}
