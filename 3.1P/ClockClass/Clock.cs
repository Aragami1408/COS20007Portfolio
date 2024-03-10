namespace COS20007Portfolio;

public class Clock
{
    private Counter _hours;
    private Counter _minutes;
    private Counter _seconds;

    public Clock()
    {
        _hours = new Counter("Hours");
        _minutes = new Counter("Minutes");
        _seconds = new Counter("Seconds");

        _hours.Reset();
        _minutes.Reset();
        _seconds.Reset();
    }

    public void Tick()
    {
        _seconds.Increment();
    }

    public void Reset()
    {

    }

    public string ToString()
    {
        throw new NotImplementedException("");
    }

    public Counter Hours
    {
        get { return _hours; }
        set { _hours = value; }
    }

    public Counter Minutes
    {
        get { return _minutes; }
        set { _minutes = value; }
    }

    public Counter Seconds
    {
        get { return _seconds; }
        set { _seconds = value; }
    }


}
