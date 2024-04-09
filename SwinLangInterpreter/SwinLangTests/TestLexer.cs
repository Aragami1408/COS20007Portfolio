namespace SwinLangTests;

using NUnit.Framework;
using SwinLangCore;

public class TestLexer
{
    [Test]
    public void TestNextToken()
    {
        string input = @"let five = 5;
let ten = 10;

let add = fn(x, y) {
    x + y;
};

let result = add(five, ten);
";

        Token[] tests = new Token[]
        {
            // let five = 5;
            new Token(TokenType.LET, "let"),
            new Token(TokenType.IDENT, "five"),
            new Token(TokenType.ASSIGN, "="),
            new Token(TokenType.INT, "5"),
            new Token(TokenType.SEMICOLON, ";"),

            // let ten = 10;
            new Token(TokenType.LET, "let"),
            new Token(TokenType.IDENT, "ten"),
            new Token(TokenType.ASSIGN, "="),
            new Token(TokenType.INT, "10"),
            new Token(TokenType.SEMICOLON, ";"),

            /*
                let add = fn(x, y) {
                    x + y;
                };
            */
            new Token(TokenType.LET, "let"),
            new Token(TokenType.IDENT, "add"),
            new Token(TokenType.ASSIGN, "="),
            new Token(TokenType.FUNCTION, "fn"),
            new Token(TokenType.LPAREN, "("),
            new Token(TokenType.IDENT, "x"),
            new Token(TokenType.COMMA, ","),
            new Token(TokenType.IDENT, "y"),
            new Token(TokenType.RPAREN, ")"),
            new Token(TokenType.LBRACE, "{"),
            new Token(TokenType.IDENT, "x"),
            new Token(TokenType.PLUS, "+"),
            new Token(TokenType.IDENT, "y"),
            new Token(TokenType.SEMICOLON, ";"),
            new Token(TokenType.RBRACE, "}"),
            new Token(TokenType.SEMICOLON, ";"),

            // let result = add(five, ten);
            new Token(TokenType.LET, "let"),
            new Token(TokenType.IDENT, "result"),
            new Token(TokenType.ASSIGN, "="),
            new Token(TokenType.IDENT, "add"),
            new Token(TokenType.LPAREN, "("),
            new Token(TokenType.IDENT, "five"),
            new Token(TokenType.COMMA, ","),
            new Token(TokenType.IDENT, "ten"),
            new Token(TokenType.RPAREN, ")"),
            new Token(TokenType.SEMICOLON, ";"),
        };

        Lexer l = new Lexer(input);

        foreach (Token expected in tests)
        {
            Token actual = l.NextToken();
            Assert.That(actual.Type, Is.EqualTo(expected.Type));
            Assert.That(actual.Literal, Is.EqualTo(expected.Literal));
        }
    }
}
