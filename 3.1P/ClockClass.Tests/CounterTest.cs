using COS20007Portfolio;
namespace ClockClass.Tests;

public class CounterTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void CounterInitializationTest()
    {
        Counter counter = new Counter("Test Counter");
        Assert.That(0, Is.EqualTo(counter.Ticks));
    }

    [Test]
    public void CounterIncrementByOneTest()
    {
        Counter counter = new Counter("Test Counter");
        counter.Increment();

        Assert.That(1, Is.EqualTo(counter.Ticks));
    }

    [Test]
    public void CounterIncrementTest()
    {
        Counter counter = new Counter("Test Counter");
        for (int i = 0; i < 10; i++) 
        {
            counter.Increment();
        } 

        Assert.That(10, Is.EqualTo(counter.Ticks));
    }

    [Test]
    public void CounterResetTest()
    {
        Counter counter = new Counter("Test Counter");

        counter.Increment();
        Assert.That(1, Is.EqualTo(counter.Ticks));

        counter.Reset();
        Assert.That(0, Is.EqualTo(counter.Ticks));

    }
    

}
