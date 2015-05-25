using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneCog.Io.Philips.Hue;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Disposables;
using System.Reactive;

namespace OneCog.Io.Philips.Hue.Tests
{
    [TestFixture]
    public class ReactiveTestFixture
    {
        private static KeyValuePair<string, int> A1 = new KeyValuePair<string, int>("A", 1);
        private static KeyValuePair<string, int> A2 = new KeyValuePair<string, int>("A", 2);
        private static KeyValuePair<string, int> A3 = new KeyValuePair<string, int>("A", 3);

        private static KeyValuePair<string, int> B1 = new KeyValuePair<string, int>("B", 1);
        private static KeyValuePair<string, int> B2 = new KeyValuePair<string, int>("B", 2);
        private static KeyValuePair<string, int> B3 = new KeyValuePair<string, int>("B", 3);

        private static KeyValuePair<string, int> C1 = new KeyValuePair<string, int>("C", 1);
        private static KeyValuePair<string, int> C2 = new KeyValuePair<string, int>("C", 2);
        private static KeyValuePair<string, int> C3 = new KeyValuePair<string, int>("C", 3);
        
        [Test]
        public void ShouldReturnAllItemsWhileCold()
        {
            Subject<KeyValuePair<string, int>> source = new Subject<KeyValuePair<string, int>>();
            
            List<KeyValuePair<string, int>> expected = new List<KeyValuePair<string,int>>(new [] { A1, B1, A2, C1 });
            List<KeyValuePair<string, int>> actual = new List<KeyValuePair<string,int>>();

            IConnectableObservable<KeyValuePair<string, int>> observable = source.GroupLatest(kvp => kvp.Key);

            using (observable.Connect())
            {
                using (observable.Subscribe(actual.Add))
                {
                    source.OnNext(A1);
                    source.OnNext(B1);
                    source.OnNext(A2);
                    source.OnNext(C1);
                }
            }

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void ShouldReturnGroupedItemsWhenConnected()
        {
            Subject<KeyValuePair<string, int>> source = new Subject<KeyValuePair<string, int>>();

            List<KeyValuePair<string, int>> expected = new List<KeyValuePair<string, int>>(new[] { B1, A2, C1 });
            List<KeyValuePair<string, int>> actual = new List<KeyValuePair<string, int>>();

            IConnectableObservable<KeyValuePair<string, int>> observable = source.GroupLatest(kvp => kvp.Key);

            using (observable.Connect())
            {
                source.OnNext(A1);
                source.OnNext(B1);
                source.OnNext(A2);
                source.OnNext(C1);

                using (observable.Subscribe(actual.Add)) { }
            }

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void ShouldReturnGroupedThenAllItemsWhenConnected()
        {
            Subject<KeyValuePair<string, int>> source = new Subject<KeyValuePair<string, int>>();

            List<KeyValuePair<string, int>> expected = new List<KeyValuePair<string, int>>(new[] { A2, B1, C1, B2, A3, C2 });
            List<KeyValuePair<string, int>> actual = new List<KeyValuePair<string, int>>();

            IConnectableObservable<KeyValuePair<string, int>> observable = source.GroupLatest(kvp => kvp.Key);

            using (observable.Connect())
            {
                source.OnNext(A1);
                source.OnNext(B1);
                source.OnNext(A2);
                source.OnNext(C1);

                using (observable.Subscribe(actual.Add)) 
                {
                    source.OnNext(B2);
                    source.OnNext(A3);
                    source.OnNext(C2);
                }
            }

            CollectionAssert.AreEquivalent(expected, actual);
        }
    }
}
