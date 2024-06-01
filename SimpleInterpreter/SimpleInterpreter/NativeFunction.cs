namespace SimpleInterpreter;

public class NativeFunction : ICallable
{
  public NativeFunction(string name, Func<Interpreter, IReadOnlyList<object>, object> action, int arity = 0) {
    Name = name;
    Arity = arity;
    this.action = action;
  }

  public string Name {get;}
  private readonly Func<Interpreter, IReadOnlyList<object>, object> action;
  public int Arity { get; }

  public object call(Interpreter interpreter, params object[] arguments)
  {
    return action(interpreter, arguments);
  }

  public override string ToString()
  {
    return $"<native fn>";
  }
}
