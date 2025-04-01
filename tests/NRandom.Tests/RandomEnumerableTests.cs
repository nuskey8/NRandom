using NRandom.Collections;
using NRandom.Linq;

namespace NRandom.Tests;

public class RandomEnumerableTests
{
    [Test]
    public void Test_ToWeightedList()
    {
        var list1 = Enumerable.Range(0, 100)
            .ToWeightedList(x => x);

        var list2 = new WeightedList<int>();
        for (int i = 0; i < 100; i++)
        {
            list2.Add(i, i);
        }

        CollectionAssert.AreEqual(list1, list2);
    }
}