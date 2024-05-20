namespace SimpleInterpreter;

public class Environment
{
  public readonly Environment enclosing;
  private readonly Dictionary<string, object> values = new Dictionary<string, object>();

  public Environment()
  {
    enclosing = null;
  }

  public Environment(Environment enclosing)
  {
    this.enclosing = enclosing;
  }

  public void define(string name, object value)
  {
    values.Add(name, value); 
  }

  public Environment ancestor(int distance)
  {
    Environment environment = this;


    for (int i = 0; i < distance; i++)
      environment = environment.enclosing;

    return environment;
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

  public object getAt(int distance, string name)
  {
    Environment anc = ancestor(distance);
    return anc.values[name];
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


  public void assignAt(int distance, Token name, object value)
  {
    Environment anc = ancestor(distance);
    anc.values.Add(name.lexeme, value);
  }


}
