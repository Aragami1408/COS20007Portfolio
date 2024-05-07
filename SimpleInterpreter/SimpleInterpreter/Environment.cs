namespace SimpleInterpreter;

public class Environment
{
  private Dictionary<string, object> values;

  public Environment()
  {
    values = new Dictionary<string, object>(); 
  }

  public void define(string name, object value)
  {
    values.Add(name, value); 
  }

  public object get(Token name)
  {
    if (values.ContainsKey(name.Lexeme))
    {
      return values[name.Lexeme];
    }

    throw new RuntimeError(name, "Undefined variable '" + name.Lexeme + "'.");
  }

  public void assign(Token name, object value)
  {
    if (values.ContainsKey(name.Lexeme))
    {
      values[name.Lexeme] = value;
      return;
    }

    throw new RuntimeError(name, "Undefined variable '"  + name.Lexeme + "'.");
  }
}
