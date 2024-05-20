namespace SimpleInterpreter;

public class Resolver : Expr.Visitor<object>, Stmt.Visitor<object>
{
    private enum FunctionType
    {
	NONE,
	FUNCTION,
	METHOD
    }

    private readonly Interpreter _interpreter;
    private readonly Stack<Dictionary<string, bool>> _scopes = new Stack<Dictionary<string, bool>>();
    private FunctionType _currentFunction = FunctionType.NONE;

    public Resolver(Interpreter interpreter)
    {
	this._interpreter = interpreter;
    }

#region "Private Helpers"

    private void beginScope()
    {
	_scopes.Push(new Dictionary<string, bool>());
    }

    public void resolve(List<Stmt> statements)
    {
	foreach (Stmt statement in statements)
	{
	    resolve(statement);
	}
    }

    private void resolve(Stmt stmt)
    {
	stmt.accept(this);
    }

    private void resolve(Expr expr)
    {
	expr.accept(this);
    }

    private void resolveLocal(Expr expr, Token name)
    {
	for (int i = _scopes.Count - 1; i >=0; i--)
	{
	    if (_scopes.ToArray()[i].ContainsKey(name.lexeme))
	    {
		_interpreter.resolve(expr, _scopes.Count - 1 - i);
		return;
	    }
	}

	//Not found locally.  Assume global.
    }

    private void resolveFunction(Stmt.Function function, FunctionType type)
    {
	FunctionType enclosingFunction = _currentFunction;
	_currentFunction = type;

	beginScope();
	foreach (Token param in function.parameters)
	{
	    declare(param);
	    define(param);
	}

	resolve(function.body);
	endScope();

	_currentFunction = enclosingFunction;
    }

    private void declare(Token name)
    {
	if (_scopes.Count == 0) return;

	Dictionary<string, bool> scope = _scopes.Peek();
	if (scope.ContainsKey(name.lexeme))
	{
	    Program.error(name, "Variable with this name already declared in this scope.");
	}
	else
	{
	    scope.Add(name.lexeme, false);
	}            
    }

    private void define(Token name)
    {
	if (_scopes.Count == 0) return;

	if (_scopes.Peek().ContainsKey(name.lexeme))
	{
	    _scopes.Peek()[name.lexeme] = true;
	}
	else
	{
	    Program.error(name, "Variable has not been declared.");
	}

    }

    private void endScope()
    {
	_scopes.Pop();
    }
    
#endregion

#region "Statement Visitors"

    public object visitBlockStmt(Stmt.Block stmt)
    {
	beginScope();
	resolve(stmt.statements);
	endScope();

	return null;
    }

    public object visitClassStmt(Stmt.Class stmt)
    {
	declare(stmt.name);
	define(stmt.name);

	beginScope();
	_scopes.Peek().Add("this", true);

	foreach (Stmt.Function method in stmt.methods)
	{
	    FunctionType declaration = FunctionType.METHOD;
	    resolveFunction(method, declaration);
	}

	endScope();

	return null;
    }

    public object visitExpressionStmt(Stmt.Expression stmt)
    {
	resolve(stmt.expression);

	return null;
    }

    public object visitFunctionStmt(Stmt.Function stmt)
    {
	declare(stmt.name);
	define(stmt.name);

	resolveFunction(stmt, FunctionType.FUNCTION);

	return null;
    }

    public object visitIfStmt(Stmt.If stmt)
    {
	resolve(stmt.condition);
	resolve(stmt.thenBranch);
	if (stmt.elseBranch != null) resolve(stmt.elseBranch);

	return null;
    }

    public object visitPrintStmt(Stmt.Print stmt)
    {
	resolve(stmt.expression);

	return null;
    }

    public object visitReturnStmt(Stmt.Return stmt)
    {
	if (_currentFunction == FunctionType.NONE)
	    Program.error(stmt.keyword, "Can't return form top-level code");

	if (stmt.value != null)
	    resolve(stmt.value);

	return null;
    }

    public object visitVarStmt(Stmt.Var stmt)
    {
	declare(stmt.name);

	if (stmt.initializer != null)
	    resolve(stmt.initializer);

	define(stmt.name);
	return null;
    }

    public object visitWhileStmt(Stmt.While stmt)
    {
	resolve(stmt.condition);
	resolve(stmt.body);
	return null;
    }

#endregion

#region "Expression Visitors"

    public object visitAssignExpr(Expr.Assign expr)
    {
	resolve(expr.value);
	resolveLocal(expr, expr.name);
	return null;
    }

    public object visitBinaryExpr(Expr.Binary expr)
    {
	resolve(expr.left);
	resolve(expr.right);
	return null;
    }

    public object visitCallExpr(Expr.Call expr)
    {
	resolve(expr.callee);

	foreach (Expr argument in expr.arguments)
	{
	    resolve(argument);
	}

	return null;
    }

    public object visitGetExpr(Expr.Get expr)
    {
	resolve(expr.obj);
	return null;
    }

    public object visitGroupingExpr(Expr.Grouping expr)
    {
	resolve(expr.expression);
	return null;
    }

    public object visitLiteralExpr(Expr.Literal expr)
    {
	return null;
    }

    public object visitVariableExpr(Expr.Variable expr)
    {
	if (_scopes.Count != 0 && _scopes.Peek().TryGetValue(expr.name.lexeme, out bool value) && value == false)
	{
	    Program.error(expr.name, "Can't read local variable in its own initializer.");
	}

	resolveLocal(expr, expr.name);
	return null;
    }

    public object visitLogicalExpr(Expr.Logical expr)
    {
	resolve(expr.left);
	resolve(expr.right);
	return null;
    }

    public object visitSetExpr(Expr.Set expr)
    {
	resolve(expr.value);
	resolve(expr.obj);
	return null;
    }

    public object visitThisExpr(Expr.This expr)
    {
	resolveLocal(expr, expr.keyword);
	return null;
    }

    public object visitUnaryExpr(Expr.Unary expr)
    {
	resolve(expr.right);
	return null;
    }

#endregion
}
