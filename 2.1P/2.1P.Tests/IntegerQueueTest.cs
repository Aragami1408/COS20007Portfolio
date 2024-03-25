using NUnit.Framework;

namespace COS20007Portfolio.Tests 
{

    public class Tests
    {
        [Test]
        public void Enqueue()
        {
            IntegerQueue queue = new IntegerQueue();

            queue.Enqueue(12345);
            int count = queue.Count;

            Assert.That(count, Is.EqualTo(1));

            // accessing member variables directly
            Assert.That(queue._elements.Count, Is.EqualTo(1));
            Assert.That(queue._elements[0], Is.EqualTo(12345));
        }

        [Test]
        public void Dequeue()
        {
            Assert.Fail();
        }
    }
}

