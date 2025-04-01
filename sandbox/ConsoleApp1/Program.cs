using NRandom.Linq;

foreach (var i in RandomEnumerable.Repeat(0L, 10000L, 100))
{
    Console.WriteLine(i);
}