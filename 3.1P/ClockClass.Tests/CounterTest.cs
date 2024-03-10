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
        Assert.AreEqual(0, counter.Ticks);
    }

    [Test]
    public void CounterIncrementByOneTest()
    {
        Counter counter = new Counter("Test Counter");
        counter.Increment();

        Assert.AreEqual(1, counter.Ticks);
    }

    [Test]
    public void CounterIncrementTest()
    {
        Counter counter = new Counter("Test Counter");
        for (int i = 0; i < 10; i++) 
        {
            counter.Increment();
        } 

        Assert.AreEqual(10, counter.Ticks);
    }

    [Test]
    public void CounterResetTest()
    {
        Counter counter = new Counter("Test Counter");

        counter.Increment();
        Assert.AreEqual(1, counter.Ticks);

        counter.Reset();
        Assert.AreEqual(0, counter.Ticks);

    }
    

}