namespace SimpleInterpreter;

using System.Collections.Generic;

public abstract class Stmt
{
	public interface Visitor<R>
	{
		 R visitExpressionStmt(Expression stmt);
		 R visitPrintStmt(Print stmt);
	}
	public class Expression : Stmt
	{
		public Expression(Expr expression)
		{
			this.expression = expression;
		}

		public override R accept<R>(Visitor<R> visitor)
		{
			return visitor.visitExpressionStmt(this);
		}

		public Expr expression;
	}
	public class Print : Stmt
	{
		public Print(Expr expression)
		{
			this.expression = expression;
		}

		public override R accept<R>(Visitor<R> visitor)
		{
			return visitor.visitPrintStmt(this);
		}

		public Expr expression;
	}

	public abstract R accept<R>(Visitor<R> visitor);
}
