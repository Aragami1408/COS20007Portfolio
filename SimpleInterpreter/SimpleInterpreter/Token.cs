namespace SimpleInterpreter;

public enum TokenType
{
  NONE,
  // single character tokens
  LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE,
  COMMA, DOT, MINUS, PLUS, SEMICOLON, SLASH, STAR,
  PERCENT,
  // one or two character tokens
  BANG, BANG_EQUAL,
  EQUAL, EQUAL_EQUAL,
  GREATER, GREATER_EQUAL,
  LESS, LESS_EQUAL,
  PLUS_EQUAL, MINUS_EQUAL, // += and -= not implemented yet
  // literals
  IDENTIFIER, STRING, NUMBER,
  // keywords
  AND, CLASS, ELSE, FALSE, FUN, FOR, IF, NIL, OR,
  PRINT, RETURN, SUPER, THIS, TRUE, VAR, WHILE, BREAK,

  EOF
}

public class Token
{
  public TokenType type;
  public string lexeme;
  public dynamic literal;
  public int line;

  public Token(TokenType type, string lexeme, dynamic literal, int line)
  {
    this.type = type;
    this.lexeme = lexeme;
    this.literal = literal;
    this.line = line;
  }

  public override string? ToString()
  {
    return type + " " + lexeme + " " + literal;
  }
}
