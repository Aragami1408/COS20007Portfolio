namespace SwinLangCore;

public class Token
{
    private string _type;
    private string _literal;

    public string Type { get { return _type;} set {_type = value;} }
    public string Literal { get { return _literal;} set {_literal = value;} }

    public Token()
    {
        Type = "";
        Literal = "";
    }

    public Token(string type, string literal)
    {
        Type = type;
        Literal = literal;
    }

}

public static class TokenType
{
    public const string ILLEGAL = "ILLEGAL";
    public const string EOF = "EOF";
    // Identifiers + literals
    public const string IDENT = "IDENT"; // add, foobar, x, y, ...
    public const string INT = "INT";
    // Operators
    public const string ASSIGN = "=";
    public const string PLUS = "+";
    // Delimiters
    public const string COMMA = ",";
    public const string SEMICOLON = ";";
    public const string LPAREN = "(";
    public const string RPAREN = ")";
    public const string LBRACE = "{";
    public const string RBRACE = "}";
    // Keywords
    public const string FUNCTION = "FUNCTION";
    public const string LET = "LET";
}

public static class TokenKeyword
{
    public static readonly Dictionary<string, string> keywords = new Dictionary<string, string>()
    {
        {"fn", TokenType.FUNCTION},
        {"let", TokenType.LET}
    };

    public static string LookupIdent(string ident)
    {
        if (keywords.ContainsKey(ident))
        {
            return keywords[ident];
        }

        return TokenType.IDENT;
    }
}
