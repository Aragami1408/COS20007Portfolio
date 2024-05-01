namespace SimpleInterpreter;

public enum TokenType
{
  NONE,
  // single character tokens
  LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE,
  COMMA, DOT, MINUS, PLUS, SEMICOLON, SLASH, STAR,
  // one or two character tokens
  BANG, BANG_EQUAL,
  EQUAL, EQUAL_EQUAL,
  GREATER, GREATER_EQUAL,
  LESS, LESS_EQUAL,
  // literals
  IDENTIFIER, STRING, NUMBER,
  // keywords
  AND, CLASS, ELSE, FALSE, FUN, FOR, IF, NIL, OR,
  PRINT, RETURN, SUPER, THIS, TRUE, VAR, WHILE,

  EOF
}

public class Token
{
  private TokenType _type;
  private string _lexeme;
  private dynamic _literal;
  private int _line;

  public Token(TokenType type, string lexeme, dynamic literal, int line)
  {
    _type = type;
    _lexeme = lexeme;
    _literal = literal;
    _line = line;
  }

  public TokenType Type
  {
    get { return _type; }
    set { _type = value; }
  }

  public string Lexeme
  {
    get { return _lexeme; }
    set { _lexeme = value; }
  }

  public dynamic Literal
  {
    get { return _literal; }
    set { _literal = value; }
  }

  public int Line
  {
    get { return _line; }
    set { _line = value; }
  }

  public override string? ToString()
  {
    return Type + " " + Lexeme + " " + Literal;
  }
}
