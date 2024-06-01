namespace SimpleInterpreter;

public class Program 
{
  public static bool hadError = false;
  public static bool hadRuntimeError = false;
  public static Interpreter interpreter = new Interpreter();

  static void Main(string[] args)
  {
    if (args.Length > 1) 
    {
      Console.WriteLine("Usage: simp [script]");
      System.Environment.Exit(1);
    }
    else if (args.Length == 1) 
    {
      runFile(args[0]);
    }
    else
    {
      runPrompt();
    }
  }

  static void runFile(string path)
  {
    StreamReader sr = new StreamReader(path);
    run(sr.ReadToEnd());

    if (hadError) System.Environment.Exit(65);
    if (hadRuntimeError) System.Environment.Exit(70);
  }

  static void runPrompt()
  {
    for (;;)
    {
      hadError = false;
      Console.Write("> ");
      string? line = Console.ReadLine();
      if (line == null) break;
      run(line);
    }
  }

  static void run(string source)
  {
    Scanner scanner = new Scanner(source);
    List<Token> tokens = scanner.scanTokens();
    Parser parser = new Parser(tokens);
    List<Stmt> statements = parser.parse();

    if (hadError) return;

    // Stop if there was a resolution error
    if (hadError) return;

    interpreter.interpret(statements);
  }

  public static void error(int line, string message)
  {
    report(line, "", message);
  }

  public static void runtimeError(RuntimeError error)
  {
    Console.Error.WriteLine(error.Message + "\n[line " + error.token.line + "]");
    hadRuntimeError = true;
  }

  public static void report(int line, string where, string message)
  {
    Console.Error.WriteLine("[line " + line + "] Error" + where + ": " + message);
    hadError = true;
  }

  public static void error(Token token, string message)
  {
    if (token.type == TokenType.EOF) {
      report(token.line, " at end", message);
    } else {
      report(token.line, " at '" + token.lexeme + "'", message);
    }

  }
}
