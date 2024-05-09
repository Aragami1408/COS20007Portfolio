namespace SimpleInterpreter;

public class Environment
{
  private Environment enclosing;
  public Dictionary<string, object> values;

  public Environment()
  {
    enclosing = null;
    values = new Dictionary<string, object>(); 
  }

  public Environment(Environment enclosing)
  {
    this.enclosing = enclosing;
    values = new Dictionary<string, object>();  
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
