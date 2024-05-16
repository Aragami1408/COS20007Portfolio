namespace SimpleInterpreter;

public class SimpReturn : SystemException
{
    public object value;

    public SimpReturn(object value) : base()
    {
	this.value = value;
    }
}
