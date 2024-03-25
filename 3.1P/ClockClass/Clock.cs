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
        if (_seconds.Ticks == 60) {
            _minutes.Increment();
            _seconds.Reset();
            if (_minutes.Ticks == 60) {
                _hours.Increment();
                _minutes.Reset();
                if (_hours.Ticks == 24) {
                    Reset();
                }
            }
        }
    }

    public void Reset()
    {
        _seconds.Reset();
        _minutes.Reset();
        _hours.Reset();
    }

    public string CurrentTime()
    {
        return String.Format("{0:00}:{1:00}:{2:00}", _hours.Ticks, _minutes.Ticks, _seconds.Ticks);
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
