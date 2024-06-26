﻿namespace SimpleInterpreter;

public class Parser
{
  private sealed class ParseError : SystemException {}
  private List<Token> _tokens;
  private int current = 0;
  private int loopDepth = 0;

  public List<Token> Tokens { get => _tokens; set => _tokens = value; }

  public Parser(List<Token> tokens)
  {
    _tokens = tokens;
  }

  public List<Stmt> parse()
  {
    List<Stmt> statements = new List<Stmt>();
    try 
    {
      while (!isAtEnd())
        statements.Add(declaration());
    }
    catch (ParseError) 
    {
      return null;
    }

    return statements;
  }

  private Stmt declaration()
  {
    try
    {
      if (match(TokenType.VAR)) return varDeclaration();
      if (match(TokenType.FUN)) return function("function");

      return statement();
    }
    catch (ParseError error)
    {
      synchronize();
      return null;
    }
  }

  private Stmt varDeclaration()
  {
    Token name = consume(TokenType.IDENTIFIER, "Expect variable name");

    Expr initializer = null;
    if (match(TokenType.EQUAL))
      initializer = expression();

    consume(TokenType.SEMICOLON, "Expect ';' after variable declaration");
    return new Stmt.Var(name, initializer);
  }

  private Stmt.Function function(string kind)
  {
    Token name = consume(TokenType.IDENTIFIER, "Expect " + kind + " name.");
    consume(TokenType.LEFT_PAREN, "Expect '(' after " + kind + " name.");
    List<Token> parameters = new List<Token>();

    if (!check(TokenType.RIGHT_PAREN))
    {
      do
      {
        if (parameters.Count >= 255)
          error(peek(), "Can't have more than 255 parameters");

        parameters.Add(consume(TokenType.IDENTIFIER, "Expect parameter name"));
      } while (match(TokenType.COMMA));
    }

    consume(TokenType.RIGHT_PAREN, "Expect ')' after parameters");
    consume(TokenType.LEFT_BRACE, "Expect '{' before " + kind + " body.");

    List<Stmt> body = block();
    return new Stmt.Function(name, parameters, body);
  }

  private Stmt statement()
  {
    if (match(TokenType.BREAK)) return breakStatement();
    if (match(TokenType.IF)) return ifStatement();
    if (match(TokenType.PRINT)) return printStatement();
    if (match(TokenType.RETURN)) return returnStatement();
    if (match(TokenType.WHILE)) return whileStatement();
    if (match(TokenType.FOR)) return forStatement();
    if (match(TokenType.LEFT_BRACE)) return new Stmt.Block(block());

    return expressionStatement();
  }

  private Stmt breakStatement() {
    if (loopDepth == 0) 
    {
      error(previous(), "Must be inside a loop to use break");
    }

    consume(TokenType.SEMICOLON, "Expect ';' after 'break'.");

    return new Stmt.Break();
  }

  private Stmt expressionStatement()
  {
    Expr expr = expression();
    consume(TokenType.SEMICOLON, "Expect ';' after expression");
    return new Stmt.Expression(expr); 
  }

  private Stmt ifStatement()
  {
    consume(TokenType.LEFT_PAREN, "Expect '(' after if");
    Expr condition = expression();
    consume(TokenType.RIGHT_PAREN, "Expect ')' after if condition");

    Stmt thenBranch = statement();

    Stmt elseBranch = null;
    if (match(TokenType.ELSE))
      elseBranch = statement();

    return new Stmt.If(condition, thenBranch, elseBranch);
  }

  private Stmt printStatement()
  {
    Expr val = expression();
    consume(TokenType.SEMICOLON, "Expect ';' after value");
    return new Stmt.Print(val);
  }

  private Stmt returnStatement()
  {
    Token keyword = previous();

    Expr value = null;
    if (!check(TokenType.SEMICOLON))
    {
      value = expression();
    }

    consume(TokenType.SEMICOLON, "Expect ';' after return value");
    return new Stmt.Return(keyword, value);
  }

  private Stmt whileStatement()
  {
    try 
    {
      loopDepth++;
      consume(TokenType.LEFT_PAREN, "Expect '(' after while");
      Expr condition = expression();
      consume(TokenType.RIGHT_PAREN, "Expect ')' after condition");
      Stmt body = statement();

      return new Stmt.While(condition, body);
    }
    finally
    {
      loopDepth--;
    }
  }

  private Stmt forStatement()
  {

    try 
    {
      loopDepth++;

      consume(TokenType.LEFT_PAREN, "Expect '(' after for");

      Stmt initializer;
      if (match(TokenType.SEMICOLON))
        initializer = null;
      else if (match(TokenType.VAR))
        initializer = varDeclaration();
      else
        initializer = expressionStatement();

      Expr condition = null;
      if(!match(TokenType.SEMICOLON)) {
        condition = expression();
        consume(TokenType.SEMICOLON, "Expect ';' after loop condition");
      }

      Expr increment = null;
      if (!match(TokenType.RIGHT_PAREN)) {
        increment = expression();
        consume(TokenType.RIGHT_PAREN, "Expect ')' after for clauses");
      }
      Stmt body = statement();
      if (increment != null)
      {
        body = new Stmt.Block(new List<Stmt>(new Stmt[] {
              body,
              new Stmt.Expression(increment)
              }));
      }

      if (condition == null) 
        condition = new Expr.Literal(true);
      body = new Stmt.While(condition, body);

      if (initializer != null)
        body = new Stmt.Block(new List<Stmt>(new Stmt[] {
              initializer,
              body
              }));

      return body;
    }
    finally
    {
      loopDepth--;
    }
  }

  private List<Stmt> block()
  {
    List<Stmt> statements = new List<Stmt>();

    while (!check(TokenType.RIGHT_BRACE) && !isAtEnd())
      statements.Add(declaration());

    consume(TokenType.RIGHT_BRACE, "Expect '}' after block");
    return statements;
  }


  private Expr expression()
  {
    return assignment();
  }

  private Expr assignment()
  {
    Expr expr = or();

    if (match(TokenType.EQUAL) || match(TokenType.PLUS_EQUAL) || match(TokenType.MINUS_EQUAL))
    {
      Token equals = previous();
      Expr value = assignment();

      if (expr is Expr.Variable variable)
      {
        Token name = variable.name;
        return new Expr.Assign(name, equals, value);
      }

      error(equals, "Invalid assignment target.");
    }

    return expr;
  }

  private Expr or()
  {
    Expr expr = and();

    while (match(TokenType.OR))
    {
      Token op = previous();
      Expr right = and();
      expr = new Expr.Logical(expr, op, right);
    }

    return expr;
  }

  private Expr and()
  {
    Expr expr = equality();

    while (match(TokenType.AND))
    {
      Token op = previous();
      Expr right = equality();
      expr = new Expr.Logical(expr, op, right);
    }

    return expr;
  }

  private Expr equality()
  {
    Expr expr = comparison();

    while (match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
    {
      Token op = previous();
      Expr right = comparison();
      expr = new Expr.Binary(expr, op, right);
    }

    return expr;
  }

  private Expr comparison()
  {
    Expr expr = term();

    while (match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
    {
      Token op = previous();
      Expr right = term();
      expr = new Expr.Binary(expr, op, right);
    }

    return expr;
  }

  private Expr term()
  {
    Expr expr = factor();

    while (match(TokenType.PLUS, TokenType.MINUS))
    {
      Token op = previous();
      Expr right = factor();
      expr = new Expr.Binary(expr, op, right);
    }

    return expr;
  }

  private Expr factor()
  {
    Expr expr = unary();

    while (match(TokenType.STAR, TokenType.SLASH, TokenType.PERCENT))
    {
      Token op = previous();
      Expr right = unary();
      expr = new Expr.Binary(expr, op, right);
    }

    return expr;
  }

  private Expr unary()
  {
    if (match(TokenType.BANG, TokenType.MINUS))
    {
      Token op = previous();
      Expr right = unary();
      return new Expr.Unary(op, right);
    }

    return call();
  }

  private Expr finishCall(Expr callee)
  {
    List<Expr> arguments = new List<Expr>();
    if (!check(TokenType.RIGHT_PAREN))
    {
      do
      {
        if (arguments.Count >= 255)
        {
          error(peek(), "Can't have more than 255 arguments");
        }
        arguments.Add(expression());
      } while (match(TokenType.COMMA));
    }

    Token paren = consume(TokenType.RIGHT_PAREN, "Expect ')' after arguments");

    return new Expr.Call(callee, paren, arguments);
  }

  private Expr call()
  {
    Expr expr = primary();

    while (true)
    {
      if (match(TokenType.LEFT_PAREN))
        expr = finishCall(expr);
      else
        break;
    }

    return expr;
  }

  private Expr primary()
  {
    if (match(TokenType.FALSE)) return new Expr.Literal(false);
    if (match(TokenType.TRUE)) return new Expr.Literal(true);
    if (match(TokenType.NIL)) return new Expr.Literal(null);

    if (match(TokenType.NUMBER, TokenType.STRING))
      return new Expr.Literal(previous().literal);


    if (match(TokenType.IDENTIFIER))
      return new Expr.Variable(previous());

    if (match(TokenType.LEFT_PAREN))
    {
      Expr expr = expression();
      consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
      return new Expr.Grouping(expr);
    }

    throw error(peek(), "Expect expression");
  }

  private bool match(params TokenType[] types)
  {
    foreach (TokenType type in types)
    {
      if (check(type))
      {
        advance();
        return true;
      }
    }

    return false;
  }

  private Token consume(TokenType type, string message)
  {
    if (check(type)) return advance();

    throw error(peek(), message);
  }

  private bool check(TokenType type)
  {
    if (isAtEnd()) return false;
    return peek().type == type;
  }

  private Token advance()
  {
    if (!isAtEnd()) current++;
    return previous();
  }

  private bool isAtEnd()
  {
    return peek().type == TokenType.EOF;
  }

  private Token peek()
  {
    return Tokens.ElementAt(current);
  }

  private Token previous()
  {
    return Tokens.ElementAt(current - 1);
  }

  private ParseError error(Token token, String message)
  {
    Program.error(token, message);
    return new ParseError();
  }

  private void synchronize()
  {
    advance();

    while (!isAtEnd())
    {
      if (previous().type == TokenType.SEMICOLON) return;

      switch (peek().type)
      {
        case TokenType.CLASS:
        case TokenType.FUN:
        case TokenType.VAR:
        case TokenType.FOR:
        case TokenType.IF:
        case TokenType.WHILE:
        case TokenType.PRINT:
        case TokenType.RETURN:
          return;
      }

      advance();
    }
  }

}
