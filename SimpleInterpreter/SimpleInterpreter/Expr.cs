namespace SimpleInterpreter;

using System.Collections.Generic;

public abstract class Expr
{
	public interface Visitor<R>
	{
		 R visitBinaryExpr(Binary expr);
		 R visitGroupingExpr(Grouping expr);
		 R visitLiteralExpr(Literal expr);
		 R visitUnaryExpr(Unary expr);
	}
	public class Binary : Expr
	{
		public Binary(Expr left, Token op, Expr right)
		{
			this.left = left;
			this.op = op;
			this.right = right;
		}

		public override R accept<R>(Visitor<R> visitor)
		{
			return visitor.visitBinaryExpr(this);
		}

		public Expr left;
		public Token op;
		public Expr right;
	}
	public class Grouping : Expr
	{
		public Grouping(Expr expression)
		{
			this.expression = expression;
		}

		public override R accept<R>(Visitor<R> visitor)
		{
			return visitor.visitGroupingExpr(this);
		}

		public Expr expression;
	}
	public class Literal : Expr
	{
		public Literal(Object value)
		{
			this.value = value;
		}

		public override R accept<R>(Visitor<R> visitor)
		{
			return visitor.visitLiteralExpr(this);
		}

		public Object value;
	}
	public class Unary : Expr
	{
		public Unary(Token op, Expr right)
		{
			this.op = op;
			this.right = right;
		}

		public override R accept<R>(Visitor<R> visitor)
		{
			return visitor.visitUnaryExpr(this);
		}

		public Token op;
		public Expr right;
	}

	public abstract R accept<R>(Visitor<R> visitor);
}
