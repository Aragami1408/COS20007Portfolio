using COS20007Portfolio;
namespace ClockClass.Tests;

public class ClockTest
{
    [Test]
    public void ClockInitializationTest()
    {
        Clock clock = new Clock();
        string currentTime = clock.CurrentTime();
        Assert.That(currentTime, Is.EqualTo("00:00:00"));
    }

    [Test]
    public void ClockTickTest() 
    {
        Clock clock = new Clock();

        clock.Tick();
        string currentTime = clock.CurrentTime();
        Assert.That(currentTime, Is.EqualTo("00:00:01"));
        
    }

    [Test]
    public void ClockTickMinutesTest()
    {
        // Arrange
        Clock clock = new Clock();

        for (int i = 1; i <= 59; i++)
            clock.Tick();

        // Act
        clock.Tick();
        string currentTime = clock.CurrentTime();

        // Assert
        Assert.That(currentTime, Is.EqualTo("00:01:00"));
    }

    [Test]
    public void TestClockTickHours() 
    {
        Clock clock = new Clock();

        for (int i = 1; i <= 3599; i++)
            clock.Tick();

        clock.Tick();
        string CurrentTime = clock.CurrentTime();

        Assert.That(CurrentTime, Is.EqualTo("01:00:00"));
    }

    [Test]
    public void ClockDone24HoursTest() 
    {
        Clock clock = new Clock();

        for (int i = 1; i <= 86399; i++)
            clock.Tick();

        clock.Tick();
        string CurrentTime = clock.CurrentTime();

        Assert.That(CurrentTime, Is.EqualTo("00:00:00"));    
    }

    [Test]
    public void ClockResetTest()
    {
        // Arrange
        Clock clock = new Clock();
        // Setting the clock to 12:34:56 
        for (int i = 1; i <= 45296; i++)
            clock.Tick();

        // Act
        clock.Reset();
        string currentTimeAfterReset = clock.CurrentTime();

        // Assert
        Assert.AreEqual("00:00:00", currentTimeAfterReset);
    }


}
