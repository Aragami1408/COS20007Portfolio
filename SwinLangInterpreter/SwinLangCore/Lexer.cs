namespace SwinLangCore;

public class Lexer
{
    private string _input;
    private int _position; // current position in input (points to current char)
    private int _readPosition; // current reading position in input (after current char)
    private char _currentChar; // current char under examination

    public string Input
    {
        get { return _input; }
        set { _input = value; }
    }

    public int Position
    {
        get { return _position; }
        set { _position = value; }
    }

    public int ReadPosition
    {
        get { return _readPosition; }
        set { _readPosition = value; }
    }

    public char CurrentChar
    {
        get { return _currentChar; }
        set { _currentChar = value; }
    }

    public Lexer(string input)
    {
        Input = input; 
        Position = 0;
        ReadPosition = 0;
        CurrentChar = '\0';
        ReadChar();
    }

    public Token NextToken()
    {
        Token tok = new Token();

        SkipWhitespace();
        switch (CurrentChar)
        {
            case '=':
                tok = NewToken(TokenType.ASSIGN, CurrentChar);
                break;
            case ';':
                tok = NewToken(TokenType.SEMICOLON, CurrentChar);
                break;
            case '(':
                tok = NewToken(TokenType.LPAREN, CurrentChar);
                break;
            case ')':
                tok = NewToken(TokenType.RPAREN, CurrentChar);
                break;
            case ',':
                tok = NewToken(TokenType.COMMA, CurrentChar);
                break;
            case '+':
                tok = NewToken(TokenType.PLUS, CurrentChar);
                break;
            case '{':
                tok = NewToken(TokenType.LBRACE, CurrentChar);
                break;
            case '}':
                tok = NewToken(TokenType.RBRACE, CurrentChar);
                break;
            case '\0':
                tok.Type = TokenType.EOF;
                tok.Literal = "";
                break;
            default:
                if (IsLetter(CurrentChar))
                {
                    tok.Literal = ReadIdentifier();
                    tok.Type = TokenKeyword.LookupIdent(tok.Literal);
                    return tok;
                }
                else if (IsDigit(CurrentChar))
                {
                    tok.Type = TokenType.INT;
                    tok.Literal = ReadNumber();
                    return tok;
                }
                else
                {
                    tok = NewToken(TokenType.ILLEGAL, CurrentChar);
                }
                break;
        }
        ReadChar(); 
        return tok;
    }

    private void ReadChar()
    {
        if (ReadPosition >= Input.Length)
        {
            CurrentChar = '\0';
        }
        else
        {
            CurrentChar = Convert.ToChar(Input[ReadPosition]);
        }
        Position = ReadPosition;
        ReadPosition += 1;
    }

    private string ReadIdentifier()
    {
        int position = Position;
        while (IsLetter(CurrentChar))
        {
            ReadChar();
        }
        return Input.Substring(position, Position);
    }

    private string ReadNumber()
    {
        int position = Position;
        while (IsDigit(CurrentChar))
            ReadChar();
        return Input.Substring(position, Position);
    }

    private void SkipWhitespace()
    {
        while (CurrentChar == ' ' || CurrentChar == '\t' || CurrentChar == '\n' || CurrentChar == '\r')
        {
            ReadChar();
        }
    }

    private bool IsLetter(char ch)
    {
        return char.IsLetter(ch) || ch == '_';
    }

    private bool IsDigit(char ch)
    {
        return char.IsDigit(ch);
    }

    private Token NewToken(string tokenType, char ch)
    {
        return new Token(tokenType, Convert.ToString(ch));
    }
}
