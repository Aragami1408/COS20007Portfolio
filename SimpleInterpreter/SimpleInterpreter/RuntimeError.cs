namespace SimpleInterpreter;

public class RuntimeError : SystemException
{
	public Token token;

	public RuntimeError(Token tok, string message) : base(message)
	{
		this.token = tok;
	}
}
