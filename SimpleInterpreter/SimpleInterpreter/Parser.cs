namespace SimpleInterpreter;

public class Parser
{
  private sealed class ParseError : SystemException {}
  private List<Token> _tokens;
  private int current;

  public List<Token> Tokens { get => _tokens; set => _tokens = value; }

  public Parser(List<Token> tokens)
  {
    _tokens = tokens;
    current = 0;
  }

  public List<Stmt> parse()
  {
    List<Stmt> statements = new List<Stmt>();
    while (!isAtEnd())
      statements.Add(statement());
    return statements;
  }

  private Stmt statement()
  {
    if (match(TokenType.PRINT)) return printStatement();
    return expressionStatement();
  }

  private Stmt printStatement()
  {
    Expr val = expression();
    consume(TokenType.SEMICOLON, "Expect ';' after value");
    return new Stmt.Print(val);
  }

  private Stmt expressionStatement()
  {
    Expr expr = expression();
    consume(TokenType.SEMICOLON, "Expect ';' after expression");
    return new Stmt.Expression(expr); 
  }


  private Expr expression()
  {
    return equality();
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

    while (match(TokenType.STAR, TokenType.SLASH))
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

    return primary();
  }

  private Expr primary()
  {
    if (match(TokenType.FALSE)) return new Expr.Literal(false);
    if (match(TokenType.TRUE)) return new Expr.Literal(true);
    if (match(TokenType.NIL)) return new Expr.Literal(null);

    if (match(TokenType.NUMBER, TokenType.STRING))
      return new Expr.Literal(previous().Literal);

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
    return peek().Type == type;
  }

  private Token advance()
  {
    if (!isAtEnd()) current++;
    return previous();
  }

  private bool isAtEnd()
  {
    return peek().Type == TokenType.EOF;
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
      if (previous().Type == TokenType.SEMICOLON) return;

      switch (peek().Type)
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
