namespace SimpleInterpreter;

public sealed class Interpreter : Stmt.Visitor<object>, Expr.Visitor<object>
{
  public Environment _globals;

  public Environment _environment;
  private readonly Dictionary<Expr, int> _locals = new Dictionary<Expr, int>();

  private Interpreter() 
  {
    _globals = new Environment();
    this._environment = _globals;

    ClockFunc clockFunc = new ClockFunc();
    _globals.define("clock", clockFunc);
  }

  private static Interpreter _instance;

  public static Interpreter getInstance()
  {
    if (_instance == null)
      _instance = new Interpreter();
    return _instance;
  }

  public void interpret(List<Stmt> statements)
  {
    try
    {
      foreach (Stmt statement in statements)
      {
        execute(statement);
      }
    }
    catch (RuntimeError error)
    {
      Program.runtimeError(error);
    }
  }

  public void resolve(Expr expr, int depth)
  {
    _locals.Add(expr, depth);
  }

  private void execute(Stmt stmt)
  {
    stmt.accept(this);
  }

  public object visitAssignExpr(Expr.Assign expr)
  {
    object value = evaluate(expr.value);

    if (_locals.TryGetValue(expr, out int distance))
    {
      _environment.assignAt(distance, expr.name, value);
    }
    else
    {
      _globals.assign(expr.name, value);
    }

    return value;
  }

  public object visitBinaryExpr(Expr.Binary expr)
  {
    object left = evaluate(expr.left);
    object right = evaluate(expr.right);

    switch (expr.op.type)
    {
      case TokenType.GREATER:
        checkNumberOperands(expr.op, left, right);
        return (double) left > (double) right;
      case TokenType.GREATER_EQUAL:
        checkNumberOperands(expr.op, left, right);
        return (double) left >= (double) right;
      case TokenType.LESS:
        checkNumberOperands(expr.op, left, right);
        return (double) left < (double) right;
      case TokenType.LESS_EQUAL:
        checkNumberOperands(expr.op, left, right);
        return (double) left <= (double) right;

      case TokenType.BANG_EQUAL:
        return !isEqual(left, right);
      case TokenType.EQUAL_EQUAL:
        return isEqual(left, right);

      case TokenType.AND:
        return (bool)left && (bool)right;
      case TokenType.OR:
        return (bool)left || (bool)right;

      case TokenType.PLUS:
        if (left is double && right is double)
          return (double)left + (double)right;
        if (left is string && right is string)
          return (string)left + (string)right;	
        if (left is string && right is double)
          return (string)left + stringify(right);
        if (left is double && right is string)
          return stringify(left) + (string)right;
        throw new RuntimeError(expr.op, "Operands must be two numbers or two strings");
      case TokenType.MINUS:
        checkNumberOperands(expr.op, left, right);
        return (double) left - (double)right;
      case TokenType.STAR:
        checkNumberOperands(expr.op, left, right);
        return (double) left * (double)right;
      case TokenType.SLASH:
        checkNumberOperands(expr.op, left, right);
        return (double) left / (double)right;
    }

    return null;
  }

  public object visitCallExpr(Expr.Call expr)
  {
    object callee = evaluate(expr.callee);

    List<object> arguments = new List<object>();
    foreach (Expr argument in expr.arguments)
    {
      arguments.Add(evaluate(argument));
    }

    if (!(callee is ICallable))
    {
      throw new RuntimeError(expr.paren, "Can only call functions and classes.");
    }

    ICallable function = (ICallable)callee;
    if (arguments.Count != function.Arity())
    {
      throw new RuntimeError(expr.paren, 
          "Expected " 
          + function.Arity()
          + " arguments but got " 
          + arguments.Count + ".");
    }

    return function.call(this, arguments);
  }

  public object visitGetExpr(Expr.Get expr)
  {
    object obj = evaluate(expr.obj);
    if (obj is SimpInstance)
    {
      return ((SimpInstance) obj).get(expr.name);
    }

    throw new RuntimeError(expr.name, "Only instances have properties");
  }

  public object visitGroupingExpr(Expr.Grouping expr)
  {
    return evaluate(expr.expression);
  }

  public object visitLiteralExpr(Expr.Literal expr)
  {
    return expr.value;
  }

  public object visitLogicalExpr(Expr.Logical expr)
  {
    object left = evaluate(expr.left);
    
    if (expr.op.type == TokenType.OR)
    {
      if (isTruthy(left))
        return left;
    }
    else
    {
      if (!isTruthy(left))
        return left;
    }

    return evaluate(expr.right);
  }

  public object visitSetExpr(Expr.Set expr)
  {
    object obj = evaluate(expr.obj);

    if (!(obj is SimpInstance))
      throw new RuntimeError(expr.name, "Only instances have fields");

    object value = evaluate(expr.value);
    ((SimpInstance) obj).set(expr.name, value);

    return value;
  }

  public object visitThisExpr(Expr.This expr)
  {
    return lookUpVariable(expr.keyword, expr);
  }

  public object visitVariableExpr(Expr.Variable expr)
  {
    return lookUpVariable(expr.name, expr);
  }

  public void executeBlock(List<Stmt> statements, Environment env)
  {
    Environment previous = this._environment;

    try
    {
      this._environment = env;

      foreach (Stmt statement in statements)
        execute(statement);
    }
    finally
    {
      this._environment = previous;
    }
  }

  public object visitExpressionStmt(Stmt.Expression stmt)
  {
    evaluate(stmt.expression);

    return null;
  }

  public object visitPrintStmt(Stmt.Print stmt)
  {
    object value = evaluate(stmt.expression);
    Console.WriteLine(stringify(value));

    return null;
  }

  public object visitReturnStmt(Stmt.Return stmt)
  {
    object value = null;

    if (stmt.value != null) 
      value = evaluate(stmt.value);

    throw new SimpReturn(value);
  }

  public object visitVarStmt(Stmt.Var stmt)
  {
    object value = null;
    if (stmt.initializer != null)
    {
      value = evaluate(stmt.initializer);
    }

    _environment.define(stmt.name.lexeme, value);
    return null;
  }

  public object visitFunctionStmt(Stmt.Function stmt)
  {
    SimpFunction function = new SimpFunction(stmt, _environment);
    _environment.define(stmt.name.lexeme, function);
    return null;
  }

  public object visitBlockStmt(Stmt.Block stmt)
  {
    executeBlock(stmt.statements, new Environment(_environment));
    return null;
  }

  public object visitClassStmt(Stmt.Class stmt)
  {
    _environment.define(stmt.name.lexeme, null);

    Dictionary<string, SimpFunction> methods = new Dictionary<string, SimpFunction>();
    foreach (Stmt.Function method in stmt.methods)
    {
      SimpFunction function = new SimpFunction(method, _environment);
      methods.Add(method.name.lexeme, function);
    }

    SimpClass simpClass = new SimpClass(stmt.name.lexeme, methods);

    _environment.assign(stmt.name, simpClass);
    return null;
  }

  public object visitIfStmt(Stmt.If stmt)
  {
    if (isTruthy(evaluate(stmt.condition)))
      execute(stmt.thenBranch);
    else if (stmt.elseBranch != null)
      execute(stmt.elseBranch);

    return null;
  }

  public object visitWhileStmt(Stmt.While stmt)
  {
    while (isTruthy(evaluate(stmt.condition)))
    {
      execute(stmt.body);
    }
    return null;
  }


  public object visitUnaryExpr(Expr.Unary expr)
  {
    object right = evaluate(expr.right);

    switch (expr.op.type)
    {
      case TokenType.BANG:
        return !isTruthy(right);
      case TokenType.MINUS:
        checkNumberOperand(expr.op, right);
        return -(double)right;
    }

    return null;
  }

  private object lookUpVariable(Token name, Expr expr)
  {
    if (_locals.TryGetValue(expr, out int distance))
    {
      return _environment.getAt(distance, name.lexeme);
    }
    else
    {
      return _globals.get(name);
    }
  }

  private object evaluate(Expr expr)
  {
    return expr.accept(this);
  }

  private string stringify(object obj)
  {
    if (obj == null) return "nil";

    if (obj is double)
    {
      string text = obj.ToString();
      if (text.EndsWith(".0"))
      {
        text = text.Substring(0, text.Length - 2);
      }

      return text;
    }

    if (obj is bool)
    {
      if ((bool) obj) return "true";
      else return "false";
    }

    return obj.ToString();
  }

  private void checkNumberOperand(Token op, object operand)
  {
    if (operand is double) return;
    throw new RuntimeError(op, "operand must be a number.");
  }

  private void checkNumberOperands(Token op, object left, object right)
  {
    if (left is double && right is double) return;
    throw new RuntimeError(op, "operands must be numbers.");
  }

  private bool isTruthy(object obj)
  {
    if (obj == null) return false;
    if (obj is bool) return (bool)obj;
    return true;
  }

  private bool isEqual(object a, object b)
  {
    if (a == null && b == null) return true;
    if (a == null) return false;

    return a.Equals(b);
  }

}
