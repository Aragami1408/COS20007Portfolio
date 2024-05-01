namespace SimpleInterpreter;


public class Scanner
{
  private string _source;
  private List<Token> _tokens;
  private int _start;
  private int _current;
  private int _line;
  private Dictionary<string, TokenType> _keywords;

  public Scanner(string source)
  {
    _source = source;
    _tokens = new List<Token>();
    _start = 0;
    _current = 0;
    _line = 1;
    _keywords = new Dictionary<string, TokenType>();

    _keywords.Add("and", TokenType.AND);
    _keywords.Add("class", TokenType.CLASS);
    _keywords.Add("else", TokenType.ELSE);
    _keywords.Add("false", TokenType.FALSE);
    _keywords.Add("for", TokenType.FOR);
    _keywords.Add("fun", TokenType.FUN);
    _keywords.Add("if", TokenType.IF);
    _keywords.Add("nil", TokenType.NIL);
    _keywords.Add("or", TokenType.OR);
    _keywords.Add("print", TokenType.PRINT);
    _keywords.Add("return", TokenType.RETURN);
    _keywords.Add("super", TokenType.SUPER);
    _keywords.Add("this", TokenType.THIS);
    _keywords.Add("true", TokenType.TRUE);
    _keywords.Add("var", TokenType.VAR);
    _keywords.Add("while", TokenType.WHILE);
  }

  public string Source { get => _source; set => _source = value; }
  public List<Token> Tokens { get => _tokens; set => _tokens = value; }
  public int Start { get => _start; set => _start = value; }
  public int Current { get => _current; set => _current = value; }
  public int Line { get => _line; set => _line = value; }
  public Dictionary<string, TokenType> Keywords { get => _keywords; set => _keywords = value; }

  public List<Token> scanTokens()
  {
    while (!isAtEnd())
    {
      Start = Current;
      scanToken();
    }
    return Tokens;
  }

  private bool match(char expected)
  {
    if (isAtEnd()) return false;
    if (Source[Current] != expected) return false;

    Current++;
    return true;
  }

  private char peek()
  {
    if (isAtEnd()) return '\0';
    return Source[Current];
  }

  private char peekNext()
  {
    if (Current + 1 >= Source.Length) return '\0';
    return Source[Current + 1];
  }

  private bool isAlpha(char c) 
  {
    return (c >= 'a' && c <= 'z') ||
      (c >= 'A' && c <= 'Z') ||
      c == '_';
  }

  private bool isAlphaNumeric(char c) 
  {
    return isAlpha(c) || isDigit(c);
  }

  private bool isDigit(char c)
  {
    return c >= '0' && c <= '9';
  }

  private void scanToken()
  {
    char c = advance();
    switch (c) {
      case '(': addToken(TokenType.LEFT_PAREN); break;
      case ')': addToken(TokenType.RIGHT_PAREN); break;
      case '{': addToken(TokenType.LEFT_BRACE); break;
      case '}': addToken(TokenType.RIGHT_BRACE); break;
      case ',': addToken(TokenType.COMMA); break;
      case '.': addToken(TokenType.DOT); break;
      case '-': addToken(TokenType.MINUS); break;
      case '+': addToken(TokenType.PLUS); break;
      case ';': addToken(TokenType.SEMICOLON); break;
      case '*': addToken(TokenType.STAR); break; 
      case '!': addToken(match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
      case '=': addToken(match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
      case '<': addToken(match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
      case '>': addToken(match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
      case '/':
                if (match('/'))
                {
                  while (peek() != '\n' && !isAtEnd()) advance();
                }
                else
                {
                  addToken(TokenType.SLASH);
                }
                break;
      case ' ':
      case '\r':
      case '\t':
                // Don't give a shit about whitespace
                break;

      case '\n':
                Line++;
                break;

      case '"':
                str();
                break;

      case 'o':
                if (match('r'))
                  addToken(TokenType.OR);
                break;

      default:
                if (isDigit(c))
                  number();
                else if (isAlpha(c))
                  identifier();
                else 
                {
                  Program.error(Line, "Unexpected character.");
                }
                break;
    }
  }

  private void identifier()
  {
    while (isAlphaNumeric(peek())) advance();
    String text = Source.Substring(Start, Current);
    TokenType type = Keywords[text];
    addToken(type);
  }

  private void number()
  {
    while (isDigit(peek())) advance();

    if (peek() == '.' && isDigit(peekNext()))
    {
      advance();

      while (isDigit(peek())) advance();
    }

    addToken(TokenType.NUMBER, Double.Parse(Source.Substring(Start, Current)));
  }

  private void str()
  {
    while (peek() != '"' && !isAtEnd())
    {
      if (peek() == '\n') Line++;
      advance();
    }

    if (isAtEnd())
    {
      Program.error(Line, "Unterminated string");
      return;
    }

    advance();

    string value = Source.Substring(Start + 1, Current - 1);
    addToken(TokenType.STRING, value);
  }


  private bool isAtEnd()
  {
    return Current >= Source.Length;
  }

  private char advance()
  {
    return Source[Current++];
  }

  private void addToken(TokenType type)
  {
    addToken(type, null);
  }

  private void addToken(TokenType type, dynamic? literal)
  {
    string text = Source.Substring(Start, Current);
    Tokens.Add(new Token(type, text, literal, Line));
  }

}
