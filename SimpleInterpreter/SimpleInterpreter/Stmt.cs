namespace SimpleInterpreter;

using System.Collections.Generic;

public abstract class Stmt
{
	public interface Visitor<R>
	{
		 R visitExpressionStmt(Expression stmt);
		 R visitPrintStmt(Print stmt);
		 R visitVarStmt(Var stmt);
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

	public class Var : Stmt
	{
		public Var(Token name, Expr initializer)
		{
			this.name = name;
			this.initializer = initializer;
		}

		public override R accept<R>(Visitor<R> visitor)
		{
			return visitor.visitVarStmt(this);
		}

		public Token name;
		public Expr initializer;
	}


	public abstract R accept<R>(Visitor<R> visitor);
}
