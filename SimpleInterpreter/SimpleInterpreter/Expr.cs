namespace SimpleInterpreter;

using System.Collections.Generic;

public abstract class Expr
{
	public interface Visitor<R>
	{
		 R visitAssignExpr(Assign expr);
		 R visitBinaryExpr(Binary expr);
		 R visitUnaryExpr(Unary expr);
		 R visitLiteralExpr(Literal expr);
		 R visitLogicalExpr(Logical expr);
		 R visitGroupingExpr(Grouping expr);
		 R visitCallExpr(Call expr);
		 R visitVariableExpr(Variable expr);
	}

	public class Assign : Expr
	{
		public Assign(Token name, Token op, Expr value)
		{
			this.name = name;
			this.op = op;
			this.value = value;
		}

		public override R accept<R>(Visitor<R> visitor)
		{
			return visitor.visitAssignExpr(this);
		}

		public Token name;
		public Token op;
		public Expr value;
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

	public class Literal : Expr
	{
		public Literal(object value)
		{
			this.value = value;
		}

		public override R accept<R>(Visitor<R> visitor)
		{
			return visitor.visitLiteralExpr(this);
		}

		public object value;
	}

	public class Logical : Expr
	{
		public Logical(Expr left, Token op, Expr right)
		{
			this.left = left;
			this.op = op;
			this.right = right;
		}

		public override R accept<R>(Visitor<R> visitor)
		{
			return visitor.visitLogicalExpr(this);
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

	public class Call : Expr
	{
		public Call(Expr callee, Token parenthesis, IEnumerable<Expr> arguments)
		{
			this.callee = callee;
			this.parenthesis = parenthesis;
			this.arguments = arguments;
		}

		public override R accept<R>(Visitor<R> visitor)
		{
			return visitor.visitCallExpr(this);
		}

		public Expr callee;
		public Token parenthesis;
		public IEnumerable<Expr> arguments;
	}

	public class Variable : Expr
	{
		public Variable(Token name)
		{
			this.name = name;
		}

		public override R accept<R>(Visitor<R> visitor)
		{
			return visitor.visitVariableExpr(this);
		}

		public Token name;
	}


	public abstract R accept<R>(Visitor<R> visitor);
}
