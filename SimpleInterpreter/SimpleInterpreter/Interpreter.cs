namespace SimpleInterpreter;

public sealed class Interpreter : Stmt.Visitor<object>, Expr.Visitor<object>
{
  public enum ExecuteResult {
    CONTINUE,
    BREAK
  }

  private readonly object breakInterrupt = new object();
  private static object undefined = new object();
  public Environment environment;
  public Environment Globals {get; private set;}


  public Interpreter() 
  {
    environment = new Environment();
    Globals = new Environment();

    Globals.define("clock",
        new NativeFunction("clock", (_, __) => new TimeSpan(DateTime.Now.Ticks).TotalSeconds));
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

  private ExecuteResult execute(Stmt stmt)
  {
    return (stmt.accept(this) == breakInterrupt) ? ExecuteResult.BREAK : ExecuteResult.CONTINUE;
  }

  public object visitAssignExpr(Expr.Assign expr)
  {
    object destination = environment.get(expr.name);
    object value = evaluate(expr.value);
    switch (expr.op.type) {
      case TokenType.EQUAL:
        environment.assign(expr.name, value);
        break;
      case TokenType.PLUS_EQUAL:
        checkNumberOperands(expr.op, destination, value);
        environment.assign(expr.name, 
            (double)destination + (double)value);
        break;
      case TokenType.MINUS_EQUAL:
        checkNumberOperands(expr.op, destination, value);
        environment.assign(expr.name, 
            (double)destination - (double)value);
        break;
      
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
        else if (left is string && right is string)
          return (string)left + (string)right;	
        else if (left is string && right is double)
          return (string)left + stringify(right);
        else if (left is double && right is string)
          return stringify(left) + (string)right;
        throw new RuntimeError(expr.op, "Operands must be two numbers or two strings");
      case TokenType.MINUS:
        checkNumberOperands(expr.op, left, right);
        return (double)left - (double)right;
      case TokenType.STAR:
        checkNumberOperands(expr.op, left, right);
        return (double)left * (double)right;
      case TokenType.SLASH:
        checkNumberOperands(expr.op, left, right);
        return (double)left / (double)right;
      case TokenType.PERCENT:
        checkNumberOperands(expr.op, left, right);
        return (double)left % (double)right;
    }

    return null;
  }

  public object visitCallExpr(Expr.Call expr)
  {
    object callee = evaluate(expr.callee);

    if (callee is ICallable function) {
      object[] arguments = expr.arguments.Select(evaluate).ToArray();

      if (arguments.Length != function.Arity) {
        throw new RuntimeError(expr.parenthesis, "Expected " + function.Arity + " arguments but got " + arguments.Length);
      }

      return function.call(this, arguments);
    }

    throw new RuntimeError(expr.parenthesis, "Can only call functions");
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

    if (expr.op.type == TokenType.OR && isTruthy(left))
    {
      return left;
    }
    else if (expr.op.type == TokenType.AND && !isTruthy(left))
    {
      return left;
    }

    return evaluate(expr.right);
  }

  public object visitVariableExpr(Expr.Variable expr)
  {
    object value = environment.get(expr.name);
    if (value == undefined) {
      throw new RuntimeError(expr.name, "The variable " + expr.name.lexeme + "has not been properly initialized"); 
    }
    return value;
  }

  public ExecuteResult executeBlock(IEnumerable<Stmt> statements, Environment env)
  {
    Environment previous = this.environment;

    try
    {
      this.environment = env;

      foreach (Stmt statement in statements) 
      {
        if(execute(statement) == ExecuteResult.BREAK)
          return ExecuteResult.BREAK;
      }
    }
    finally
    {
      this.environment = previous;
    }

    return ExecuteResult.CONTINUE;
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
    object value = undefined;
    if (stmt.initializer != null)
    {
      value = evaluate(stmt.initializer);
    }

    environment.define(stmt.name.lexeme, value);
    return null;
  }

  public object visitFunctionStmt(Stmt.Function stmt)
  {
    SimpFunction function = new SimpFunction(stmt, environment);
    environment.define(stmt.name.lexeme, function);

    return null;
  }

  public object visitBlockStmt(Stmt.Block stmt)
  {
    ExecuteResult result = executeBlock(stmt.statements, new Environment(environment));
    return result == ExecuteResult.BREAK ? breakInterrupt : null;
  }

  public object visitIfStmt(Stmt.If stmt)
  {
    object condition = evaluate(stmt.condition);

    if (isTruthy(condition)) {
      if (execute(stmt.thenBranch) == ExecuteResult.BREAK)
        return breakInterrupt;
    }
    else if (stmt.elseBranch != null) {
      if (execute(stmt.elseBranch) == ExecuteResult.BREAK)
        return breakInterrupt;
    }

    return null;
  }

  public object visitWhileStmt(Stmt.While stmt)
  {
    while (isTruthy(evaluate(stmt.condition)))
    {
      if(execute(stmt.body) == ExecuteResult.BREAK)
        break;
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

  private void checkDivideByZero(Token op, double denominator) {
    if (denominator == 0) {
      throw new RuntimeError(op, "Can't divide by zero");
    }
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

  public object visitBreakStmt(Stmt.Break stmt)
  {
    return breakInterrupt;
  }
}
