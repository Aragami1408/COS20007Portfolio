namespace SimpleInterpreter;

using System.Collections.Generic;

public abstract class Stmt
{
	public interface Visitor<R>
	{
		 R visitBlockStmt(Block stmt);
		 R visitBreakStmt(Break stmt);
		 R visitExpressionStmt(Expression stmt);
		 R visitFunctionStmt(Function stmt);
		 R visitIfStmt(If stmt);
		 R visitPrintStmt(Print stmt);
		 R visitReturnStmt(Return stmt);
		 R visitVarStmt(Var stmt);
		 R visitWhileStmt(While stmt);
	}

	public class Block : Stmt
	{
		public Block(IEnumerable<Stmt> statements)
		{
			this.statements = statements;
		}

		public override R accept<R>(Visitor<R> visitor)
		{
			return visitor.visitBlockStmt(this);
		}

		public IEnumerable<Stmt> statements;
	}

	public class Break : Stmt
	{
		public Break()
		{
		}

		public override R accept<R>(Visitor<R> visitor)
		{
			return visitor.visitBreakStmt(this);
		}

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

	public class Function : Stmt
	{
		public Function(Token name, IReadOnlyList<Token> parameters, IReadOnlyList<Stmt> body)
		{
			this.name = name;
			this.parameters = parameters;
			this.body = body;
		}

		public override R accept<R>(Visitor<R> visitor)
		{
			return visitor.visitFunctionStmt(this);
		}

		public Token name;
		public IReadOnlyList<Token> parameters;
		public IReadOnlyList<Stmt> body;
	}

	public class If : Stmt
	{
		public If(Expr condition, Stmt thenBranch, Stmt elseBranch)
		{
			this.condition = condition;
			this.thenBranch = thenBranch;
			this.elseBranch = elseBranch;
		}

		public override R accept<R>(Visitor<R> visitor)
		{
			return visitor.visitIfStmt(this);
		}

		public Expr condition;
		public Stmt thenBranch;
		public Stmt elseBranch;
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

	public class Return : Stmt
	{
		public Return(Token keyword, Expr value)
		{
			this.keyword = keyword;
			this.value = value;
		}

		public override R accept<R>(Visitor<R> visitor)
		{
			return visitor.visitReturnStmt(this);
		}

		public Token keyword;
		public Expr value;
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

	public class While : Stmt
	{
		public While(Expr condition, Stmt body)
		{
			this.condition = condition;
			this.body = body;
		}

		public override R accept<R>(Visitor<R> visitor)
		{
			return visitor.visitWhileStmt(this);
		}

		public Expr condition;
		public Stmt body;
	}


	public abstract R accept<R>(Visitor<R> visitor);
}
