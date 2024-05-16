namespace SimpleInterpreter;

public interface ICallable
{
  int Arity();
  object call(Interpreter interpreter, List<object> arguments);
}
