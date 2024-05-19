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

  public Environment ancestor(int? distance)
  {
    Environment environment = this;
    for (int i = 0; i < distance; i++)
      environment = environment.enclosing;

    return environment;
  }

  public object getAt(int? distance, string name)
  {
    return ancestor(distance).values[name];
  }

  public void assignAt(int? distance, Token name, object value)
  {
    ancestor(distance).values.Add(name.lexeme, value);
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
