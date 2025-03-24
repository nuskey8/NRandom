namespace NRandom.Tests;

public class RandomRangeTests
{
    const int IterationCount = 25000;
    static IRandom[] TestCases() => TestHelper.CreateInstances();

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    [TestCaseSource(nameof(TestCases))]
    public void Test_Range_UInt(IRandom random)
    {
        for (int i = 1; i < IterationCount; i++)
        {
            var r = random.NextUInt(0, (uint)i);
            Assert.That(r, Is.GreaterThanOrEqualTo(0u).And.LessThan(i));
        }
    }

    [Test]
    [TestCaseSource(nameof(TestCases))]
    public void Test_Range_Int(IRandom random)
    {
        for (int i = 1; i < IterationCount; i++)
        {
            var r = random.NextInt(0, i);
            Assert.That(r, Is.GreaterThanOrEqualTo(0).And.LessThan(i));
        }
    }

    [Test]
    [TestCaseSource(nameof(TestCases))]
    public void Test_Range_ULong(IRandom random)
    {
        for (int i = 1; i < IterationCount; i++)
        {
            var r = random.NextULong(0, (ulong)i);
            Assert.That(r, Is.GreaterThanOrEqualTo(0).And.LessThan(i));
        }
    }

    [Test]
    [TestCaseSource(nameof(TestCases))]
    public void Test_Range_Long(IRandom random)
    {
        for (int i = 1; i < IterationCount; i++)
        {
            var r = random.NextLong(0, i);
            Assert.That(r, Is.GreaterThanOrEqualTo(0).And.LessThan(i));
        }
    }

    [Test]
    [TestCaseSource(nameof(TestCases))]
    public void Test_Range_Float(IRandom random)
    {
        for (int i = 1; i < IterationCount; i++)
        {
            var r = random.NextFloat(0f, i);
            Assert.That(r, Is.GreaterThanOrEqualTo(0f).And.LessThan(i));
        }
    }

    [Test]
    [TestCaseSource(nameof(TestCases))]
    public void Test_Range_Double(IRandom random)
    {
        for (int i = 1; i < IterationCount; i++)
        {
            var r = random.NextDouble(0.0, i);
            Assert.That(r, Is.GreaterThanOrEqualTo(0.0).And.LessThan(i));
        }
    }
}