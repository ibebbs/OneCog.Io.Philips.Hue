using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace OneCog.Io.Philips.Hue.Tests
{
    public static class EnumerableAssert
    {
        public static void AreEqual<T>(IEnumerable<T> x, IEnumerable<T> y, IEqualityComparer<T> comparer)
        {
            x = (x ?? Enumerable.Empty<T>()).ToArray();
            y = (y ?? Enumerable.Empty<T>()).ToArray();

            Assert.That(x.Count(), Is.EqualTo(y.Count()));

            var result = x.Zip(y, (a, b) => comparer.Equals(a, b)).ToArray();

            Assert.That(result.Count(), Is.EqualTo(x.Count()));
            Assert.That(result.All(r => r), Is.True);
        }

        public static void AreEquivalent<T>(IEnumerable<T> x, IEnumerable<T> y, IEqualityComparer<T> comparer)
        {
            x = (x ?? Enumerable.Empty<T>()).ToArray();
            y = (y ?? Enumerable.Empty<T>()).ToArray();

            Assert.That(x.Count(), Is.EqualTo(y.Count()));

            var result = x.Join(y, a => a, b => b, (a, b) => true, comparer).ToArray();
            
            Assert.That(result.Count(), Is.EqualTo(x.Count()));
        }
    }
}
