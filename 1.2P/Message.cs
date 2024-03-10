namespace COS20007Portfolio
{

  public class Message
  {
    private string _text;

    public Message(string txt) 
    {
      this._text = txt;
    }

    public void Print()
    {
      Console.WriteLine(_text);
    }
  }
}

