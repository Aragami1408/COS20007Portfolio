using System.Text;

namespace SimpleInterpreter;

public class ASTPrinter : Expr.Visitor<string>
{
	public string print(Expr expr) 
	{
		return expr.accept(this);
	}


	public string visitBinaryExpr(Expr.Binary expr)
	{
		return parenthesize(expr.op.Lexeme, expr.left, expr.right);
	}

	public string visitGroupingExpr(Expr.Grouping expr)
	{
		return parenthesize("group", expr.expression);
	}

	public string visitLiteralExpr(Expr.Literal expr)
	{
		if (expr.value == null) return "nil";
		return expr.value.ToString();
	}

	public string visitUnaryExpr(Expr.Unary expr)
	{
		return parenthesize(expr.op.Lexeme, expr.right);
	}

	private string parenthesize(string name, params Expr[] exprs)
	{
		StringBuilder builder = new StringBuilder();
		builder.Append("(");
		builder.Append(name);
		foreach (Expr expr in exprs)
		{
			builder.Append(" ");
			builder.Append(expr.accept(this));
		}
		builder.Append(")");
		return builder.ToString();

	}
}
