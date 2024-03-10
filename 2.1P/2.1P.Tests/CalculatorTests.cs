using NUnit.Framework;

namespace COS20007Portfolio.Tests 
{

    public class Tests
    {

        [Test]
        public void AddShouldReturnSum()
        {
            Calculator calculator = new Calculator();

            int result = calculator.Add(2,3);

            Assert.AreEqual(5, result);
        }
    }
}

