namespace SimpleInterpreter;

public class GenerateAst 
{
  static void Main(string[] args)
  {
    if (args.Length != 1) 
    {
      Console.Error.WriteLine("Usage: dotnet run <output dir>");
      System.Environment.Exit(1); 
    }

    string outputDir = args[0];
    defineAst(outputDir, "Expr", new List<string>(new string[] {
      "Binary   : Expr left, Token op, Expr right",
      "Grouping : Expr expression",
      "Literal  : Object value",
      "Unary    : Token op, Expr right"
    }));

    defineAst(outputDir, "Stmt", new List<string>(new string[] {
      "Expression : Expr expression",
      "Print      : Expr expression"
    }));
  }

  private static void defineAst(string outputDir, string baseName, List<string> types)
  {
    string path = outputDir + "/" + baseName + ".cs";
    StreamWriter writer = new StreamWriter(path);

      writer.WriteLine("namespace SimpleInterpreter;");
      writer.WriteLine();
      writer.WriteLine("using System.Collections.Generic;");
      writer.WriteLine();

      // abstract class declaration
      writer.WriteLine("public abstract class " + baseName);
      writer.WriteLine("{");
      defineVisitor(writer, baseName, types);
      foreach (string type in types)
      {
        string className = type.Split(":")[0].Trim();
        string fields = type.Split(":")[1].Trim();
        defineType(writer, baseName, className, fields);
      }

      // base accept() method
      writer.WriteLine();
      writer.WriteLine("\tpublic abstract R accept<R>(Visitor<R> visitor);");
      writer.WriteLine("}");
      writer.Close();
  }

  private static void defineType(StreamWriter writer, string baseName, string className, string fieldList)
  {
      // subclass declaration
      writer.WriteLine("\tpublic class " + className + " : " + baseName);
      writer.WriteLine("\t{");

      // constructor
      writer.WriteLine("\t\tpublic " + className + "(" + fieldList + ")");
      writer.WriteLine("\t\t{");

      // store parameters in fields
      string[] fields = fieldList.Split(", "); 
      foreach (string field in fields)
      {
        string name = field.Split(" ")[1];
        writer.WriteLine("\t\t\tthis." + name + " = " + name + ";");
      }

      writer.WriteLine("\t\t}");

      // Visitor pattern
      writer.WriteLine();
      writer.WriteLine("\t\tpublic override R accept<R>(Visitor<R> visitor)");
      writer.WriteLine("\t\t{");
      writer.WriteLine("\t\t\treturn visitor.visit" + className + baseName + "(this);");
      writer.WriteLine("\t\t}");

      // fields
      writer.WriteLine();
      foreach (string field in fields)
      {
        writer.WriteLine("\t\tpublic " + field + ";");
      }

      writer.WriteLine("\t}");
  }

  private static void defineVisitor(StreamWriter writer, string baseName, List<string> types)
  {
    writer.WriteLine("\tpublic interface Visitor<R>");
    writer.WriteLine("\t{");
    foreach (string type in types)
    {
      string typeName = type.Split(":")[0].Trim();
      writer.WriteLine("\t\t R visit" + typeName + baseName + "(" + typeName + " " + baseName.ToLower() + ");");
    }
    writer.WriteLine("\t}");
  }
}
