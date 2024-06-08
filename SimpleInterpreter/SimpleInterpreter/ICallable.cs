namespace SimpleInterpreter;

public interface ICallable
{
  int Arity { get; }
  object call(Interpreter interpreter, params object[] arguments);
}
