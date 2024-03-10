using COS20007Portfolio;
namespace ClockClass.Tests;

public class ClockTest
{
    [Test]
    public void ClockInitializationTest()
    {
        Clock clock = new Clock();

        string currentTime = clock.ToString();

        Assert.AreEqual("00:00:00", currentTime);
    }

    [Test]
    public void ClockTickTest() 
    {
        Clock clock = new Clock();

        clock.Tick();
        string currentTimeAfterTick = clock.ToString();

        Assert.AreEqual("00:00:01", currentTimeAfterTick);
    }

    [Test]
    public void TestClockTickMinutes()
    {
        // Arrange
        Clock clock = new Clock();
        // Setting the clock to 23:59:58
        clock.Hours.Ticks = 23;
        clock.Minutes.Ticks = 59;
        clock.Seconds.Ticks = 58;

        // Act
        clock.Tick();
        string currentTimeAfterTick = clock.ToString();

        // Assert
        Assert.AreEqual("23:59:59", currentTimeAfterTick);
    }

    [Test]
    public void TestClockReset()
    {
        // Arrange
        Clock clock = new Clock();
        // Setting the clock to 12:34:56
        clock.Hours.Ticks = 12;
        clock.Minutes.Ticks = 34;
        clock.Seconds.Ticks = 56;

        // Act
        clock.Reset();
        string currentTimeAfterReset = clock.ToString();

        // Assert
        Assert.AreEqual("00:00:00", currentTimeAfterReset);
    }
}
