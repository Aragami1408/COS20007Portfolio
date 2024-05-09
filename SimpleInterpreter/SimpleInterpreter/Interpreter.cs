﻿namespace SimpleInterpreter;

public sealed class Interpreter : Stmt.Visitor<object>, Expr.Visitor<object>
{
  private Environment environment;
  private Interpreter() 
  {
    environment = new Environment();
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

  private object evaluate(Expr expr)
  {
    return expr.accept(this);
  }

  private void execute(Stmt stmt)
  {
    stmt.accept(this);
  }

  private void executeBlock(List<Stmt> statements, Environment environment)
  {
    Environment previous = this.environment;

    try
    {
      this.environment = environment;

      foreach (Stmt statement in statements)
        execute(statement);
    }
    finally
    {
      this.environment = previous;
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

  public object visitVarStmt(Stmt.Var stmt)
  {
    object value = null;
    if (stmt.initializer != null)
    {
      value = evaluate(stmt.initializer);
    }

    environment.define(stmt.name.lexeme, value);
    return null;
  }

  public object visitBlockStmt(Stmt.Block stmt)
  {
    executeBlock(stmt.statements, new Environment(environment));
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

  public object visitAssignExpr(Expr.Assign expr)
  {
    object value = evaluate(expr.value);
    environment.assign(expr.name, value);
    return null;
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

  public object visitVariableExpr(Expr.Variable expr)
  {
    return environment.get(expr.name);
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
